using AudioPlayer.Other;
using Exiled.API.Features;
using SCPSLAudioApi.AudioCore;
using System.Collections.Generic;
using static AudioPlayer.Plugin;

namespace AudioPlayer;

internal class EventHandler
{
    private List<AudioFile> LobbyPlaylist = plugin.Config.LobbyPlaylist;

    internal EventHandler()
    {
        CharacterClassManager.OnInstanceModeChanged += OnInstanceModeChanged;
        Exiled.Events.Handlers.Map.Generated += OnGenerated;
        Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
        Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
        Exiled.Events.Handlers.Server.RestartingRound += OnRestartingRound;


        //AudioEvents
        if (plugin.Config.ScpslAudioApiDebug)
        {
            AudioPlayerBase.OnTrackSelecting += OnTrackSelecting;
            AudioPlayerBase.OnTrackSelected += OnTrackSelected;
            AudioPlayerBase.OnTrackLoaded += OnTrackLoaded;
            AudioPlayerBase.OnFinishedTrack += OnFinishedTrackLog;
            Log.Warn($"SCPSLAudioApi Debug Enabled!");
        }
        //End AudioEvents
    }
    ~EventHandler()
    {
        CharacterClassManager.OnInstanceModeChanged -= OnInstanceModeChanged;
        Exiled.Events.Handlers.Map.Generated -= OnGenerated;
        Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
        Exiled.Events.Handlers.Server.RestartingRound -= OnRestartingRound;


        //AudioEvents
        if (plugin.Config.ScpslAudioApiDebug)
        {
            AudioPlayerBase.OnTrackSelecting -= OnTrackSelecting;
            AudioPlayerBase.OnTrackSelected -= OnTrackSelected;
            AudioPlayerBase.OnTrackLoaded -= OnTrackLoaded;
            AudioPlayerBase.OnFinishedTrack -= OnFinishedTrackLog;
            Log.Warn($"SCPSLAudioApi Debug Enabled!");
        }
        //End AudioEvents
    }
    internal void OnRestartingRound()
    {
        if (FakeConnectionsIds.Count > 0)
            ServerStatic.StopNextRound = ServerStatic.NextRoundAction.Restart; //DONE TEMPORARILY, I DON'T KNOW HOW TO FIX IT: https://ibb.co/9ybq127
    }
    internal void OnWaitingForPlayers()
    {
        if (plugin.Config.SpawnBot && plugin.Config.SpecialEventsEnable && LobbyPlaylist.Count > 0)
            LobbyPlaylist.RandomItem().Play(true);
    }
    internal void OnRoundStarted()
    {
        if (!plugin.LobbySong) return;

        LobbyPlaylist.RandomItem().Stop();
        plugin.LobbySong = false;
    }
    //AudioEvents
    internal void OnFinishedTrack(AudioPlayerBase playerBase, string track, bool directPlay, ref int nextQueuePos)
    {
        if (!Round.IsLobby || !plugin.Config.SpecialEventsEnable || !plugin.LobbySong || LobbyPlaylist.Count == 0)
            return;

        LobbyPlaylist[UnityEngine.Random.Range(0, LobbyPlaylist.Count)].Play(true);
    }
    internal void OnTrackSelected(AudioPlayerBase playerBase, bool directPlay, int queuePos, ref string track) =>
        Log.Info("Loading Audio (Debug SCPSLAudioApi)\n" +
            $"playerBase - {playerBase} \n" +
            $"directPlay - {directPlay} \n" +
            $"queuePos - {queuePos} \n" +
            $"track - {track} \n" +
            "");

    internal void OnTrackLoaded(AudioPlayerBase playerBase, bool directPlay, int queuePos, string track) =>
        Log.Info($"Play music {directPlay} (Debug SCPSLAudioApi) \n" +
            $"playerBase - {playerBase}\n" +
            $"directPlay - {directPlay}\n" +
            $"queuePos - {queuePos} \n" +
            $"track - {track} \n" +
            "");

    internal void OnTrackSelecting(AudioPlayerBase playerBase, bool directPlay, ref int queuePos) =>
        Log.Info($"Selecting Audio.... (Debug SCPSLAudioApi) \n" +
            $"playerBase - {playerBase} \n" +
            $"directPlay - {directPlay} \n" +
            $"queuePos - {queuePos} \n" +
            "");

    internal void OnFinishedTrackLog(AudioPlayerBase playerBase, string track, bool directPlay, ref int nextQueuePos) =>
        Log.Info($"Track Complete. (Debug SCPSLAudioApi) \n" +
            $"playerBase - {playerBase} \n" +
            $"track - {track} \n" +
            $"directPlay - {directPlay} \n" +
            $"nextQueuePos - {nextQueuePos} \n" +
            "");

    internal void OnInstanceModeChanged(ReferenceHub arg1, ClientInstanceMode arg2)
    {
        if ((arg2 != ClientInstanceMode.Unverified || arg2 != ClientInstanceMode.Host) && arg1.characterClassManager._privUserId.Contains("@audioplayerbot"))
        {
            Log.Debug($"Replaced instancemode for dummy to host.");
            arg1.characterClassManager.InstanceMode = ClientInstanceMode.Host;
        }
    }

    internal void OnGenerated()
    {
        if (FakeConnectionsIds != null) FakeConnectionsIds.Clear();
        if (plugin.Config.SpawnBot)
        {
            foreach (var cfg in plugin.Config.BotsList)
            {
                Extensions.SpawnDummy(cfg.BotName, cfg.ShowPlayerList, cfg.BadgeText, cfg.BadgeColor, cfg.BotId);
            }
        }
    }
}
