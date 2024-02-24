using Exiled.API.Features;
using MEC;
using Mirror;
using PlayerRoles;
using SCPSLAudioApi.AudioCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Utils.NonAllocLINQ;
using static AudioPlayer.Plugin;
using Object = UnityEngine.Object;

namespace AudioPlayer.Other;
public static class Extensions
{
    internal static void CreateDirectory() // Creates a directory if it does not exist.
    {
        if (!Directory.Exists(plugin.AudioPath)) Directory.CreateDirectory(plugin.AudioPath);
    }
    internal static void WarheadSoundControl(int botId, bool stopSong = true, bool CanBeStartedWarhead = false, List<AudioFile> audiolist = null)
    {
        if (!plugin.Config.WarheadStopping) return;
        if (stopSong) API.AudioController.StopAudio(DLC.SpecialEvents.WarheadStartBotId);
        if (!Warhead.CanBeStarted && CanBeStartedWarhead) return;
        DLC.SpecialEvents.WarheadStartBotId = botId;
        if (audiolist != null) PlayRandomAudioFile(plugin.Config.WarheadStoppingClip);
    }
    public static bool IsAudioBot(this ReferenceHub player) => FakeConnectionsIds.Values.Any(x => x.hubPlayer == player);
    public static bool IsAudioBot(this Player player) => FakeConnectionsIds.Values.Any(x => x.hubPlayer == player.ReferenceHub);
    public static bool TryGetAudioBot(this int BotId, out FakeConnectionList fakeConnectionList)
    {
        fakeConnectionList = GetAudioBotFakeConnectionList(BotId);
        return fakeConnectionList is not null;
    }
    public static AudioFile PlayRandomAudioFile(List<AudioFile> audioClip, bool lobbyPlaylist = false, bool noBaseEvent = false)
    {
        if (audioClip == null) audioClip = plugin.Config.LobbyPlaylist; // Solves two problems, the first is that I don't have to write lobbysong every time. The second is that there may never be a null value here :troll:
        if ((!noBaseEvent && !plugin.Config.SpecialEventsEnable) || audioClip.Count == 0) return null;

        var randomClip = audioClip.RandomItem();
        randomClip.Play();

        if (lobbyPlaylist && GetAllAudioBots().Count > 0 && Round.IsLobby) plugin.LobbySong = randomClip;

        return randomClip;
    }
    public static AudioFile PlayRandomAudioFileFromPlayer(List<AudioFile> audioClip, Player player, bool noBaseEvent = false)
    {
        if (audioClip == null) audioClip = plugin.Config.LobbyPlaylist;
        if ((!noBaseEvent && !plugin.Config.SpecialEventsEnable) || audioClip.Count == 0) return null;

        var randomClip = audioClip.RandomItem();
        randomClip.PlayFromFilePlayer(new List<int>() { player.Id });

        return randomClip;
    }
    public static bool IsThereAudioBot(this int BotId) => GetAudioBotFakeConnectionList(BotId) is not null;
    public static List<FakeConnectionList> GetAllAudioBots() => FakeConnectionsIds.Values.ToList();
    public static Player GetAudioBot(this int BotId) => Player.Get(FakeConnectionsIds.Values.FirstOrDefault(x => x.BotID == BotId).hubPlayer);
    public static Player GetAudioBot(this string BotName) => Player.Get(FakeConnectionsIds.Values.FirstOrDefault(x => x.BotName == BotName).hubPlayer);
    public static Player GetAudioBot(this NetworkIdentity fakeConnection) => Player.Get(FakeConnectionsIds.Values.FirstOrDefault(x => x.fakeConnection == fakeConnection).hubPlayer);
    public static Player GetAudioBot(this AudioPlayerBase audioPlayerBase) => Player.Get(FakeConnectionsIds.Values.FirstOrDefault(x => x.audioplayer == audioPlayerBase).hubPlayer);
    public static Player GetAudioBot(this ReferenceHub referenceHub) => Player.Get(FakeConnectionsIds.Values.FirstOrDefault(x => x.hubPlayer == referenceHub).hubPlayer);
    public static FakeConnectionList GetAudioBotFakeConnectionList(this int BotId) => FakeConnectionsIds.FirstOrDefault(x => x.Key == BotId).Value;
    public static BotsList GetAudioBotInBotsList(int id) => plugin.Config.BotsList.FirstOrDefault(x => x.BotId == id);
    public static BotsList GetAudioBotInBotsList(string name) => plugin.Config.BotsList.FirstOrDefault(x => x.BotName == name);
    // The code is officially taken from https://github.com/Exiled-Team/EXILED/pull/2313/
    public static FakeConnectionList SpawnDummy(string name = "Dedicated Server", string badgetext = "AudioPlayer BOT", string bagdecolor = "orange", int id = 99)
    {
        if (IsThereAudioBot(id))
        {
            Log.Error("This id is already in use");
            return null;
        }

        GameObject newObject = Object.Instantiate(NetworkManager.singleton.playerPrefab);
        Npc npc = new(newObject)
        {
            IsNPC = true,
        };
        try { npc.ReferenceHub.roleManager.InitializeNewRole(RoleTypeId.None, RoleChangeReason.None); } catch { }

        int IdBot = id;

        if (!RecyclablePlayerId.FreeIds.Contains(id) && RecyclablePlayerId._autoIncrement >= id)
        {
            Log.Warn($"{Assembly.GetCallingAssembly().GetName().Name} tried to spawn an NPC with a duplicate PlayerID. Using auto-incremented ID instead to avoid issues..");
            try
            { id = new RecyclablePlayerId(false).Value; }
            catch
            { id = new RecyclablePlayerId(true).Value; }
        }

        FakeConnection fakeConnection = new(id);
        NetworkServer.AddPlayerForConnection(fakeConnection, newObject);

        try { npc.ReferenceHub.authManager.UserId = $"{IdBot}@audioplayerbot"; } catch { }
        npc.ReferenceHub.nicknameSync.Network_myNickSync = name;
        Player.Dictionary.Add(newObject, npc);
        FakeConnectionsIds.Add(IdBot, new FakeConnectionList()
        {
            BotID = IdBot,
            BotName = name,
            fakeConnection = fakeConnection.identity,
            audioplayer = AudioPlayerBase.Get(npc.ReferenceHub),
            hubPlayer = npc.ReferenceHub,
        });
        Timing.CallDelayed(0.1f, () =>
        {
            try
            {
                npc.ReferenceHub.roleManager.ServerSetRole(RoleTypeId.Overwatch, RoleChangeReason.RemoteAdmin);
            }
            catch (Exception e)
            {
                Log.Error($"Error on {nameof(SpawnDummy)}: Error on set dummy role {e}");
            }
            npc.ReferenceHub.serverRoles.SetText(badgetext);
            npc.ReferenceHub.serverRoles.SetColor(bagdecolor);
        });
        return FakeConnectionsIds.FirstOrDefault(x => x.Key == id).Value;
    }
    internal static (ushort horizontal, ushort vertical) ToClientUShorts(this Quaternion rotation)
    {
        const float ToHorizontal = ushort.MaxValue / 360f;
        const float ToVertical = ushort.MaxValue / 176f;

        float fixVertical = -rotation.eulerAngles.x;

        if (fixVertical < -90f)
        {
            fixVertical += 360f;
        }
        else if (fixVertical > 270f)
        {
            fixVertical -= 360f;
        }

        float horizontal = Mathf.Clamp(rotation.eulerAngles.y, 0f, 360f);
        float vertical = Mathf.Clamp(fixVertical, -88f, 88f) + 88f;

        return ((ushort)Math.Round(horizontal * ToHorizontal), (ushort)Math.Round(vertical * ToVertical));
    }
    internal static string PathCheck(string path)
    {
        if (File.Exists(path))
        {
            Log.Debug("The full path was specified, I'm skipping it");
            return path;
        }
        else if (File.Exists(Plugin.plugin.AudioPath + "/" + path))
        {
            path = Plugin.plugin.AudioPath + "/" + path;
            Log.Debug("An incomplete path was specified, I am looking for the .ogg file in the audio folder");
            return path;
        }
        else
        {
            Log.Debug($"No files exist inside that path.\nPath: {path}");
            return path;
        }
    }
}