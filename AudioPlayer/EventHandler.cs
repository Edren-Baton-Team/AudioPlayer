using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using SCPSLAudioApi.AudioCore;
using System.Collections.Generic;
using System.Linq;
using static AudioPlayer.Plugin;
using Extensions = AudioPlayer.Other.Extensions;

namespace AudioPlayer;

internal class EventHandler
{
    internal EventHandler()
    {
        Exiled.Events.Handlers.Player.Destroying += OnDestroying;
        Exiled.Events.Handlers.Map.Generated += OnGenerated;

        Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
        Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;

        AudioPlayerBase.OnFinishedTrack += OnFinishedTrack;

        if (!plugin.Config.ScpslAudioApiDebug) return;

        AudioPlayerBase.OnTrackSelecting += OnTrackSelecting;
        AudioPlayerBase.OnTrackSelected += OnTrackSelected;
        AudioPlayerBase.OnTrackLoaded += OnTrackLoaded;
        AudioPlayerBase.OnFinishedTrack += OnFinishedTrackLog;
        Log.Warn($"SCPSLAudioApi Debug Enabled!");
    }
    internal void OnDestroying(DestroyingEventArgs ev)
    {
        if (AudioPlayerList.FirstOrDefault(p => p.Value.Player == ev.Player) is KeyValuePair<int, API.Container.AudioPlayerBot> container)
        {
            AudioPlayerList.Remove(container.Key);
        }
    }

    internal void OnGenerated()
    {
        if (AudioPlayerList != null || AudioPlayerList.Any())
        {
            AudioPlayerList.Clear();
        }

        if (plugin.Config.SpawnBot)
        {
            foreach (var cfg in plugin.Config.BotsList)
            {
                Extensions.SpawnDummy(cfg.BotName, cfg.BadgeText, cfg.BadgeColor, cfg.BotId);
            }
        }
    }
    internal void OnWaitingForPlayers() => Extensions.PlayRandomAudioFile(null, true);
    internal void OnRoundStarted()
    {
        if (plugin.LobbySong != null)
        {
            plugin.LobbySong.Stop(true);
        }
    }
    internal void OnFinishedTrack(AudioPlayerBase playerBase, string track, bool directPlay, ref int nextQueuePos)
    {
        if (!Round.IsLobby)
        {
            return;
        }
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
