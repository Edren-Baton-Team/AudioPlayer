using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Components;
using Mirror;
using PlayerRoles;
using SCPSLAudioApi.AudioCore;
using System;
using System.IO;
using System.Linq;
using UnityEngine;
using Utils.NonAllocLINQ;
using static AudioPlayer.Plugin;

namespace AudioPlayer.Other;
public static class Extensions
{
    internal static void CreateDirectory() //Creates a directory if it does not exist.
    {
        if (!Directory.Exists(plugin.AudioPath))
            Directory.CreateDirectory(plugin.AudioPath);
    }
    public static bool IsAudioBot(this ReferenceHub player) => FakeConnectionsIds.Values.Any(x => x.hubPlayer == player);
    public static bool IsAudioBot(this Player player) => FakeConnectionsIds.Values.Any(x => x.hubPlayer == player.ReferenceHub);
    public static bool TryGetAudioBot(this int BotId, out FakeConnectionList fakeConnectionList)
    {
        fakeConnectionList = GetAudioBotFakeConnectionList(BotId);
        return fakeConnectionList is not null;
    }
    public static bool IsThereAudioBot(this int BotId) => GetAudioBotFakeConnectionList(BotId) is not null;
    public static Player GetAudioBot(this int BotId) => Player.Get(FakeConnectionsIds.Values.FirstOrDefault(x => x.BotID == BotId).hubPlayer);
    public static Player GetAudioBot(this string BotName) => Player.Get(FakeConnectionsIds.Values.FirstOrDefault(x => x.BotName == BotName).hubPlayer);
    public static Player GetAudioBot(this NetworkIdentity fakeConnection) => Player.Get(FakeConnectionsIds.Values.FirstOrDefault(x => x.fakeConnection == fakeConnection).hubPlayer);
    public static Player GetAudioBot(this AudioPlayerBase audioPlayerBase) => Player.Get(FakeConnectionsIds.Values.FirstOrDefault(x => x.audioplayer == audioPlayerBase).hubPlayer);
    public static Player GetAudioBot(this ReferenceHub referenceHub) => Player.Get(FakeConnectionsIds.Values.FirstOrDefault(x => x.hubPlayer == referenceHub).hubPlayer);
    public static FakeConnectionList GetAudioBotFakeConnectionList(this int BotId) => FakeConnectionsIds.FirstOrDefault(x => x.Key == BotId).Value;
    public static BotsList GetAudioBotInBotsList(int id) => plugin.Config.BotsList.FirstOrDefault(x => x.BotId == id);
    public static BotsList GetAudioBotInBotsList(string name) => plugin.Config.BotsList.FirstOrDefault(x => x.BotName == name);
    public static void SpawnDummy(string name = "Dedicated Server", bool showplayer = false, string badgetext = "AudioPlayer BOT", string bagdecolor = "orange", int id = 99)
    {
        if (IsThereAudioBot(id))
        {
            Log.Error("This id is already in use");
            return;
        }
        GameObject newPlayer = UnityEngine.Object.Instantiate(NetworkManager.singleton.playerPrefab);
        FakeConnection fakeConnection = new FakeConnection(id);
        ReferenceHub hubPlayer = newPlayer.GetComponent<ReferenceHub>();
        FakeConnectionsIds.Add(id, new FakeConnectionList()
        {
            BotID = id,
            BotName = name,
            fakeConnection = fakeConnection.identity,
            audioplayer = AudioPlayerBase.Get(hubPlayer),
            hubPlayer = hubPlayer,
        });
        if (RecyclablePlayerId.FreeIds.Contains(id))
        {
            RecyclablePlayerId.FreeIds.RemoveFromQueue(id);
        }
        else if (RecyclablePlayerId._autoIncrement >= id)
        {
            RecyclablePlayerId._autoIncrement = id = RecyclablePlayerId._autoIncrement + 1;
        }
        NetworkServer.AddPlayerForConnection(fakeConnection, newPlayer);
        if (!showplayer)
        {
            try
            {
                hubPlayer.characterClassManager.UserId = $"{id}@audioplayerbot";
            }
            catch (Exception)
            {
                //Ignore
            }
        }

        try
        {
            hubPlayer.nicknameSync.Network_myNickSync = name;
        }
        catch (Exception)
        {
            //Ignore
        }
        MEC.Timing.CallDelayed(1, () =>
        {
            try
            {
                hubPlayer.roleManager.ServerSetRole(RoleTypeId.Overwatch, RoleChangeReason.RemoteAdmin);
            }
            catch (Exception e)
            {
                Log.Error($"Error on {nameof(SpawnDummy)}: Error on set dummy role {e}");
            }
            Player.Get(hubPlayer).IsNPC = true;
            hubPlayer.characterClassManager.GodMode = true;
            hubPlayer.serverRoles.SetText(badgetext);
            hubPlayer.serverRoles.SetColor(bagdecolor);
        });

    }
}