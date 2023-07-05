using AudioPlayer.Other;
using AudioPlayer.Other.EventsArgs;
using Exiled.API.Features;
using Mirror;
using PlayerRoles;
using SCPSLAudioApi.AudioCore;
using System.Collections.Generic;
using static AudioPlayer.Plugin;

namespace AudioPlayer;

internal class EventHandler
{
    private List<AudioFile> LobbyPlaylist = plugin.Config.LobbyPlaylist;
    internal void OnWaitingForPlayers()
    {
        plugin.FakeConnectionsIds.Clear();
        if (plugin.Config.SpawnBot)
        {
            foreach (var cfg in plugin.Config.BotsList)
            {
                SpawnDummy(cfg.BotName, cfg.BadgeText, cfg.BadgeColor, cfg.BotId);
            }

            if (plugin.Config.SpecialEventsEnable)
            {
                if (LobbyPlaylist.Count > 0)
                    LobbyPlaylist[UnityEngine.Random.Range(0, LobbyPlaylist.Count)].Play(true);
            }
        }
    }
    internal void SpawnDummy(string name = "Dedicated Server", string badgetext = "AudioPlayer BOT", string bagdecolor = "orange", int id = 99)
    {
        var hubPlayer = Npc.Spawn(name, RoleTypeId.Overwatch, id, $"{id}@audioplayerbot");
        if (plugin.FakeConnectionsIds.ContainsKey(id))
        {
            Log.Error("This id is already in use");
            return;
        }
        plugin.FakeConnectionsIds.Add(id, new FakeConnectionList()
        {
            BotID = id,
            BotName = name,
            fakeConnection = hubPlayer.ReferenceHub.GetComponent<NetworkIdentity>(),
            audioplayer = AudioPlayerBase.Get(hubPlayer.ReferenceHub),
            hubPlayer = hubPlayer.ReferenceHub,
        });
        hubPlayer.IsGodModeEnabled = true;
        hubPlayer.RankName = badgetext;
        hubPlayer.RankColor = bagdecolor;
    }
    internal void OnRoundStarted()
    {
        if (plugin.LobbySong)
        {
            LobbyPlaylist[UnityEngine.Random.Range(0, LobbyPlaylist.Count)].Stop();
            plugin.LobbySong = false;
        }
    }
    //AudioEvents
    internal void OnFinishedTrack(AudioPlayerBase playerBase, string track, bool directPlay, ref int nextQueuePos)
    {
        if (!Round.IsLobby || !plugin.Config.SpecialEventsEnable || !plugin.LobbySong)
            return;

        if (LobbyPlaylist.Count > 0)
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
    internal void OnAudioPlayerDiedAttacker(AudioPlayerDiedAttackerEventArgs ev)
    {
        Log.Debug($"OnAudioPlayerDiedAttacker\nPlayer - {ev.Player} | BotList {ev.BotsList} | Path - {ev.Path}");
    }

    internal void OnAudioPlayerDiedTarget(AudioPlayerDiedTargetEventArgs ev)
    {
        Log.Debug($"OnAudioPlayerDiedTarget\nPlayer - {ev.Player} | BotList {ev.BotsList} | Path - {ev.Path}");
    }

    internal void HandleInstanceModeChange(ReferenceHub arg1, ClientInstanceMode arg2)
    {
        if (arg1.characterClassManager.UserId.Contains("@audioplayerbot"))
        {
            Log.Debug($"Replaced instancemode for dummy to host.");
            arg1.characterClassManager.InstanceMode = ClientInstanceMode.Host;
        }
    }
}
