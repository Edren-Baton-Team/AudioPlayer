using Exiled.API.Features;
using Mirror;
using PlayerRoles;
using PlayerRoles.FirstPersonControl;
using SCPSLAudioApi.AudioCore;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AudioPlayer;

public class EventHandler
{
    private List<AudioFile> LobbyPlaylist = Plugin.plugin.Config.LobbyPlaylist;
    public void OnWaitingForPlayers()
    {
        if (Plugin.plugin.Config.SpawnBot)
        {
            foreach (var cfg in Plugin.plugin.Config.BotsList)
            {
                SpawnDummy(cfg.BotName, cfg.ShowPlayerList, cfg.BadgeText, cfg.BadgeColor, cfg.BotId);
            }

            if (Plugin.plugin.Config.SpecialEventsEnable)
            {
                if (LobbyPlaylist.Count > 0)
                    LobbyPlaylist[UnityEngine.Random.Range(0, LobbyPlaylist.Count)].Play(true);
            }
        }
    }
    public void SpawnDummy(string name = "Dedicated Server", bool showplayer = false, string badgetext = "AudioPlayer BOT", string bagdecolor = "orange", int id = 99)
    {
        if (Plugin.plugin.FakeConnectionsIds.ContainsKey(id))
        {
            Log.Error("This id is already in use");
            return;
        }
        var newPlayer = UnityEngine.Object.Instantiate(NetworkManager.singleton.playerPrefab);
        var fakeConnection = new FakeConnection(id);
        var hubPlayer = newPlayer.GetComponent<ReferenceHub>();
        Plugin.plugin.FakeConnectionsIds.Add(id, new FakeConnectionList()
        {
            BotID = id,
            BotName = name,
            fakeConnection = fakeConnection,
            audioplayer = AudioPlayerBase.Get(hubPlayer),
            hubPlayer = hubPlayer,
        });

        NetworkServer.AddPlayerForConnection(fakeConnection, newPlayer);
        if (showplayer)
            hubPlayer._playerId = new RecyclablePlayerId(id);
        else
        {
            try
            {
                hubPlayer.characterClassManager._privUserId = $"player{id}@server";
            }
            catch (Exception)
            {
                //Ignore
            }
        }
        hubPlayer.characterClassManager.InstanceMode = ClientInstanceMode.Host;
        try
        {
            hubPlayer.nicknameSync.SetNick(name);
        }
        catch (Exception)
        {
            //Nice ass
        }
        try
        {
            hubPlayer.roleManager.ServerSetRole(RoleTypeId.Overwatch, RoleChangeReason.RemoteAdmin);
        }
        catch (Exception e)
        {
            Log.Error($"Error on {nameof(SpawnDummy)}: Error on set dummy role {e}");
        }
        hubPlayer.characterClassManager.GodMode = true;
        hubPlayer.serverRoles.SetText(badgetext);
        hubPlayer.serverRoles.SetColor(bagdecolor);
    }
    public void OnRoundStarted()
    {
        if (Plugin.plugin.LobbySong)
        {
            LobbyPlaylist[UnityEngine.Random.Range(0, LobbyPlaylist.Count)].Stop();
            Plugin.plugin.LobbySong = false;
        }
    }
    public void OnRestartingRound() => Plugin.plugin.FakeConnectionsIds.Clear();
    //thx for the tip ced777ric#8321
    public void HandleInstanceModeChange(ReferenceHub arg1, ClientInstanceMode arg2)
    {
        foreach (FakeConnectionList fake in Plugin.plugin.FakeConnectionsIds.Values)
        {
            if ((arg2 != ClientInstanceMode.Unverified || arg2 != ClientInstanceMode.Host) && fake.hubPlayer == arg1)
            {
                Log.Debug($"Replaced instancemode for dummy to host.");
                arg1.characterClassManager.InstanceMode = ClientInstanceMode.Host;
            }
        }
    }
    //AudioEvents
    public void OnFinishedTrack(AudioPlayerBase playerBase, string track, bool directPlay, ref int nextQueuePos)
    {
        if (!Round.IsLobby || !Plugin.plugin.Config.SpecialEventsEnable || !Plugin.plugin.LobbySong)
            return;

        if (LobbyPlaylist.Count > 0)
            LobbyPlaylist[UnityEngine.Random.Range(0, LobbyPlaylist.Count)].Play(true);
    }
    public void OnTrackSelected(AudioPlayerBase playerBase, bool directPlay, int queuePos, ref string track) =>
        Log.Info("Loading Audio (Debug SCPSLAudioApi)\n" +
            $"playerBase - {playerBase} \n" +
            $"directPlay - {directPlay} \n" +
            $"queuePos - {queuePos} \n" +
            $"track - {track} \n" +
            "");

    public void OnTrackLoaded(AudioPlayerBase playerBase, bool directPlay, int queuePos, string track) =>
        Log.Info($"Play music {directPlay} (Debug SCPSLAudioApi) \n" +
            $"playerBase - {playerBase}\n" +
            $"directPlay - {directPlay}\n" +
            $"queuePos - {queuePos} \n" +
            $"track - {track} \n" +
            "");

    public void OnTrackSelecting(AudioPlayerBase playerBase, bool directPlay, ref int queuePos) =>
        Log.Info($"Selecting Audio.... (Debug SCPSLAudioApi) \n" +
            $"playerBase - {playerBase} \n" +
            $"directPlay - {directPlay} \n" +
            $"queuePos - {queuePos} \n" +
            "");

    public void OnFinishedTrackLog(AudioPlayerBase playerBase, string track, bool directPlay, ref int nextQueuePos) =>
        Log.Info($"Track Complete. (Debug SCPSLAudioApi) \n" +
            $"playerBase - {playerBase} \n" +
            $"track - {track} \n" +
            $"directPlay - {directPlay} \n" +
            $"nextQueuePos - {nextQueuePos} \n" +
            "");
}
