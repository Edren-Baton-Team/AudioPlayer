using Exiled.API.Features;
using SCPSLAudioApi.AudioCore;
using SCPSLAudioApi;
using System;
using Mirror;
using PlayerRoles;
using PlayerRoles.FirstPersonControl;
using UnityEngine;
using HarmonyLib;
using System.IO;
using PlayerRoles.Voice;
using InventorySystem;
using System.Collections.Generic;
using VoiceChat;
using VoiceChat.Networking;

namespace AudioPlayer
{
    public class Plugin : Plugin<Config>
    {
        public override string Prefix => "AudioPlayer";
        public override string Name => "AudioPlayer";
        public override string Author => "Rysik5318 and Mariki";
        public override System.Version Version { get; } = new System.Version(1, 0, 0);
        public static Plugin plugin;
        public FakeConnection fakeConnection;
        public ReferenceHub hubPlayer;
        public AudioPlayerBase audioplayer;
        public Harmony _harmonyInstance;
        public readonly string AudioPath = Path.Combine($"{Paths.Plugins}", "audio");

        public override void OnEnabled()
        {
            plugin = this;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
            Startup.SetupDependencies();
            Extensions.CreateDirectory();
            try
            {
                _harmonyInstance = new Harmony($"Rysik5318.{DateTime.UtcNow.Ticks}");
                _harmonyInstance.PatchAll();
            }
            catch (Exception e)
            {
                Log.Error($"Error on patch harmony: {e.Data} -- {e.StackTrace}");
            }
            base.OnEnabled();
        }

        public void OnRoundStart()
        {
            foreach(Player player in Player.List) 
            {
                SpawnDummy(player.Id, player.Position, player.Rotation, "Батон Диджей", "Батон Диджей");
            }
        }
        public static void SpawnDummy(int id, Vector3 pos, Vector3 rot, string name, string custominfo)
        {
            var newPlayer = UnityEngine.Object.Instantiate(NetworkManager.singleton.playerPrefab);
            int idplayer = id;
            plugin.fakeConnection = new FakeConnection(idplayer++);
            plugin.hubPlayer = newPlayer.GetComponent<ReferenceHub>();
            NetworkServer.AddPlayerForConnection(plugin.fakeConnection, newPlayer);
            plugin.hubPlayer.characterClassManager._privUserId = $"{idplayer}";
            plugin.hubPlayer.characterClassManager.InstanceMode = ClientInstanceMode.Unverified;
            plugin.hubPlayer.roleManager.ServerSetRole(RoleTypeId.Tutorial, RoleChangeReason.RemoteAdmin);
            plugin.hubPlayer.characterClassManager.GodMode = true;
            plugin.hubPlayer.TryOverridePosition(pos, rot);
            plugin.hubPlayer.nicknameSync.name = name;
            plugin.hubPlayer.nicknameSync.DisplayName = name;
            plugin.hubPlayer.nicknameSync.Network_displayName = name;
            plugin.hubPlayer.nicknameSync.CustomPlayerInfo = custominfo;
            plugin.hubPlayer.roleManager.ServerSetRole(RoleTypeId.Overwatch, RoleChangeReason.RemoteAdmin);
        }
        public override void OnDisabled()
        {
            plugin = this;
            base.OnDisabled();
        }

    }
}
