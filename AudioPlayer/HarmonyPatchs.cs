using AudioPlayer.API;
using CentralAuth;
using Exiled.API.Features;
using GameObjectPools;
using HarmonyLib;
using Mirror;
using NorthwoodLib;
using PluginAPI.Core;
using PluginAPI.Events;
using RoundRestarting;
using System;
using System.Linq;
using System.Threading;
using UnityEngine;
using static Mono.Security.X509.X520;

namespace AudioPlayer;

[HarmonyPatch(typeof(ReferenceHub), nameof(ReferenceHub.Awake))]
internal static class ReferenceHubAwake
{
    private static bool Prefix(ReferenceHub __instance)
    {
        ReferenceHub.AllHubs.Add(__instance);
        ReferenceHub.HubsByGameObjects[__instance.gameObject] = __instance;
        if (NetworkServer.active)
        {
            __instance.Network_playerId = new RecyclablePlayerId(Other.Extensions.GenerateUniqueID(__instance));
            __instance.FriendlyFireHandler = new FriendlyFireHandler(__instance);
        }
        return false;
    }
}

[HarmonyPatch(typeof(RoundRestart), nameof(RoundRestart.InitiateRoundRestart))]
internal static class RestartingRound
{
    private static bool Prefix()
    {
        if (!NetworkServer.active)
        {
            throw new InvalidOperationException("Round restart can only be triggerred by the server!");
        }

        Facility.Reset();
        EventManager.ExecuteEvent(new RoundRestartEvent());
        PoolManager.Singleton.ReturnAllPoolObjects();
        if (RoundRestart.IsRoundRestarting)
        {
            return false;
        }

        RoundRestart.IsRoundRestarting = true;
        CustomLiteNetLib4MirrorTransport.DelayConnections = true;
        CustomLiteNetLib4MirrorTransport.UserIdFastReload.Clear();
        IdleMode.PauseIdleMode = true;
        try // Just in case
        {
            while (Plugin.FakeConnectionsIds.Count() > 0) // Waiting for all AudioPlayer bots to leave the server.
            {
                //Log.Info("1");
                foreach (var all in Plugin.FakeConnectionsIds.ToList())
                {
                    //Log.Info("Clear");
                    all.Value.audioplayer.OnDestroy();
                    AudioController.DisconnectDummy(all.Key);
                }
                Thread.Sleep(1000);
            }
        }
        catch { }
        if (CustomNetworkManager.EnableFastRestart)
        {
            foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
            {
                if (allHub.Mode != ClientInstanceMode.DedicatedServer)
                {
                    try
                    {
                        CustomLiteNetLib4MirrorTransport.UserIdFastReload.Add(allHub.authManager.UserId);
                    }
                    catch (Exception ex)
                    {
                        ServerConsole.AddLog("Exception occured during processing online player list for Fast Restart: " + ex.Message, ConsoleColor.Yellow);
                    }
                }
            }

            NetworkServer.SendToAll(new RoundRestartMessage(RoundRestartType.FastRestart, 0f, 0, reconnect: true, extendedReconnectionPeriod: true));
            RoundRestart.ChangeLevel(noShutdownMessage: false);
        }
        else
        {
            if (ServerStatic.StopNextRound == ServerStatic.NextRoundAction.DoNothing)
            {
                float offset = (float)RoundRestart.LastRestartTime / 1000f;
                NetworkServer.SendToAll(new RoundRestartMessage(RoundRestartType.FullRestart, offset, 0, reconnect: true, extendedReconnectionPeriod: true));
            }

            RoundRestart.ChangeLevel(noShutdownMessage: false);
        }
        return false;
    }
}

