using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using Exiled.Events.EventArgs.Warhead;
using Mirror;
using PlayerRoles;
using Respawning;
using SCPSLAudioApi.AudioCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AudioPlayer;

public class EventHandler
{
    public void OnWaitingForPlayers()
    {
        if (Plugin.plugin.Config.SpawnBot)
        {
            SpawnDummy(Plugin.plugin.Config.BotName);
            List<AudioFile> playlist = Plugin.plugin.Config.LobbyPlaylist;

            if (playlist.Count > 0)
                playlist[UnityEngine.Random.Range(0, playlist.Count)].Play(true);
        }
    }
    public void SpawnDummy(string name, int id = 99)
    {
        var newPlayer = UnityEngine.Object.Instantiate(NetworkManager.singleton.playerPrefab);
        var fakeConnection = new FakeConnection(id);
        var hubPlayer = newPlayer.GetComponent<ReferenceHub>();
        if (Plugin.plugin.FakeConnectionsIds.ContainsKey(id))
            Log.Error("This id is already in use");
        Plugin.plugin.FakeConnectionsIds.Add(id, new FakeConnectionList()
        {
            fakeConnection = fakeConnection,
            audioplayer = AudioPlayerBase.Get(hubPlayer),
            hubPlayer = hubPlayer,
        });

        NetworkServer.AddPlayerForConnection(fakeConnection, newPlayer);
        try
        {
            hubPlayer.characterClassManager.UserId = $"player{id}@server";
        }
        catch (Exception)
        {
            //Ignore
        }
        try
        {
            hubPlayer.nicknameSync.SetNick(name);
        }
        catch (Exception)
        {
            //Ignore
        }
        hubPlayer.roleManager.ServerSetRole(RoleTypeId.Overwatch, RoleChangeReason.RemoteAdmin);
    }
    // Stole the code from the old AudioPlayer :jermasus:
    public void OnRoundStarted()
    {
        if (Plugin.plugin.LobbySong)
        {
            API.API.StopAudio();
            Plugin.plugin.LobbySong = false;
        }
        List<AudioFile> playlist = Plugin.plugin.Config.RoundStartClip;

        if (playlist.Count > 0)
            playlist[UnityEngine.Random.Range(0, playlist.Count)].Play();
        Log.Debug($"Play music {playlist[UnityEngine.Random.Range(0, playlist.Count)].Path}");
    }
    public void OnRoundEnded(RoundEndedEventArgs ev)
    {
        List<AudioFile> playlist = Plugin.plugin.Config.RoundEndClip;

        if (playlist.Count > 0)
            playlist[UnityEngine.Random.Range(0, playlist.Count)].Play();
        Log.Debug($"Play music {playlist[UnityEngine.Random.Range(0, playlist.Count)].Path}");
    }
    public void OnRespawningTeam(RespawningTeamEventArgs ev)
    {
        switch (ev.NextKnownTeam)
        {
            case SpawnableTeamType.ChaosInsurgency:
                List<AudioFile> playlist = Plugin.plugin.Config.ChaosSpawnClip;

                if (playlist.Count > 0)
                    playlist[UnityEngine.Random.Range(0, playlist.Count)].Play();
                Log.Debug($"Play music {playlist[UnityEngine.Random.Range(0, playlist.Count)].Path}");
                break;
            case SpawnableTeamType.NineTailedFox:
                List<AudioFile> playlist1 = Plugin.plugin.Config.MtfSpawnClip;

                if (playlist1.Count > 0)
                    playlist1[UnityEngine.Random.Range(0, playlist1.Count)].Play();
                Log.Debug($"Play music {playlist1[UnityEngine.Random.Range(0, playlist1.Count)].Path}");
                break;
        }
    }
    //AudioEvent - Stopped Sound
    public void OnFinishedTrack(AudioPlayerBase playerBase, string track, bool directPlay, ref int nextQueuePos)
    {
        if (!Round.IsLobby)
            return;

        List<AudioFile> playlist = Plugin.plugin.Config.LobbyPlaylist;

        if (playlist.Count > 0)
        {
            playlist[UnityEngine.Random.Range(0, playlist.Count)].Play(true);
            Log.Debug($"Play music {playlist[UnityEngine.Random.Range(0, playlist.Count)].Path}");
        }
    }

    public void OnWarheadStarting(StartingEventArgs ev)
    {
        List<AudioFile> playlist = Plugin.plugin.Config.WarheadStartingClip;

        if (playlist.Count > 0)
            playlist[UnityEngine.Random.Range(0, playlist.Count)].Play();
        Log.Debug($"Play music {playlist[UnityEngine.Random.Range(0, playlist.Count)].Path}");
    }


    public void OnWarheadStopping(StoppingEventArgs ev)
    {
        List<AudioFile> playlist = Plugin.plugin.Config.WarheadStoppingClip;

        if (playlist.Count > 0)
            playlist[UnityEngine.Random.Range(0, playlist.Count)].Play();
        Log.Debug($"Play music {playlist[UnityEngine.Random.Range(0, playlist.Count)].Path}");
    }

    public void OnRestartingRound()
    {
        foreach (var au in Plugin.plugin.FakeConnectionsIds)
        {
            Plugin.plugin.FakeConnectionsIds.Remove(au.Key);
            NetworkServer.Destroy(au.Value.hubPlayer.gameObject);
        }
    }
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
}
