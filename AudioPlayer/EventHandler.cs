using AudioPlayer.Other;
using CentralAuth;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using SCPSLAudioApi.AudioCore;
using static AudioPlayer.Plugin;

namespace AudioPlayer;

internal class EventHandler
{
    internal EventHandler()
    {
        PlayerAuthenticationManager.OnInstanceModeChanged += OnInstanceModeChanged;
        Exiled.Events.Handlers.Map.Generated += OnGenerated;
        Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
        Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
        Exiled.Events.Handlers.Player.Joined += OnJoined;
        AudioPlayerBase.OnFinishedTrack += OnFinishedTrack;

        if (!plugin.Config.ScpslAudioApiDebug) return;
        AudioPlayerBase.OnTrackSelecting += OnTrackSelecting;
        AudioPlayerBase.OnTrackSelected += OnTrackSelected;
        AudioPlayerBase.OnTrackLoaded += OnTrackLoaded;
        AudioPlayerBase.OnFinishedTrack += OnFinishedTrackLog;
        Log.Warn($"SCPSLAudioApi Debug Enabled!");
    }
    ~EventHandler()
    {
        PlayerAuthenticationManager.OnInstanceModeChanged -= OnInstanceModeChanged;
        Exiled.Events.Handlers.Map.Generated -= OnGenerated;
        Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
        Exiled.Events.Handlers.Player.Joined -= OnJoined;
        AudioPlayerBase.OnFinishedTrack -= OnFinishedTrack;

        if (!plugin.Config.ScpslAudioApiDebug) return;
        AudioPlayerBase.OnTrackSelecting -= OnTrackSelecting;
        AudioPlayerBase.OnTrackSelected -= OnTrackSelected;
        AudioPlayerBase.OnTrackLoaded -= OnTrackLoaded;
        AudioPlayerBase.OnFinishedTrack -= OnFinishedTrackLog;
        Log.Warn($"SCPSLAudioApi Debug Enabled!");
    }
    internal void OnJoined(JoinedEventArgs ev)
    {
        if (Extensions.IsIdExists(ev.Player.Id, ev.Player.ReferenceHub))
            ev.Player.Disconnect();
    }
    internal void OnGenerated()
    {
        if (FakeConnectionsIds != null || FakeConnectionsIds.Count > 0) FakeConnectionsIds.Clear();
        if (plugin.Config.SpawnBot)
            foreach (var cfg in plugin.Config.BotsList)
                Extensions.SpawnDummy(cfg.BotName, cfg.ShowPlayerList, cfg.BadgeText, cfg.BadgeColor, cfg.BotId);
    }
    internal void OnWaitingForPlayers() => Extensions.PlayRandomAudioFile(null, true);
    internal void OnRoundStarted()
    {
        if (plugin.LobbySong != null) plugin.LobbySong.Stop(true);
    }
    internal void OnInstanceModeChanged(ReferenceHub arg1, ClientInstanceMode arg2)
    {
        if (arg1.authManager.UserId.Contains("@audioplayerbot"))
        {
            Log.Debug($"Replaced instancemode for dummy to host.");
            arg1.authManager.InstanceMode = ClientInstanceMode.Host;
        }
    }
    internal void OnFinishedTrack(AudioPlayerBase playerBase, string track, bool directPlay, ref int nextQueuePos)
    {
        if (!Round.IsLobby) return;
        Extensions.PlayRandomAudioFile(null, true);
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
}
