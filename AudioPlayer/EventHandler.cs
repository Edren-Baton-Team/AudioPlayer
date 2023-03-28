using Exiled.API.Features;
using Exiled.Events.EventArgs.Server;
using Mirror;
using PlayerRoles;
using Respawning;
using SCPSLAudioApi.AudioCore;
using System;
using System.Collections.Generic;
using System.IO;
using Utils.NonAllocLINQ;

namespace AudioPlayer;

public class EventHandler
{
    public void OnWaitingForPlayers()
    {
        List<AudioFile> playlist = Plugin.plugin.Config.LobbyPlaylist;

        if (playlist.Count > 0)
            playlist[UnityEngine.Random.Range(0, playlist.Count)].Play("LobbyPlaylist");

        if (Plugin.plugin.Config.SpawnBot)
            SpawnDummy(Plugin.plugin.Config.BotName);
    }
    public void SpawnDummy(string name, int id = 99)
    {
        var newPlayer = UnityEngine.Object.Instantiate(NetworkManager.singleton.playerPrefab);
        var fakeConnection = new FakeConnection(id);
        var hubPlayer = newPlayer.GetComponent<ReferenceHub>();
        if (Plugin.plugin.FakeConnectionsIds.ContainsKey(id))
        {
            Log.Error("This id is already in use");
        }
        Plugin.plugin.FakeConnectionsIds.Add(id, new FakeConnectionList()
        {
            fakeConnection = fakeConnection,
            audioplayer = AudioPlayerBase.Get(hubPlayer),
            hubPlayer = hubPlayer,
        });

        NetworkServer.AddPlayerForConnection(fakeConnection, newPlayer);
        try
        {
            Plugin.plugin.FakeConnectionsIds[id].hubPlayer.characterClassManager.UserId = $"player{id}@server";
        }
        catch (Exception)
        {
            //Ignore
        }
        Plugin.plugin.FakeConnectionsIds[id].hubPlayer.characterClassManager.InstanceMode = ClientInstanceMode.Host;
        try
        {
            Plugin.plugin.FakeConnectionsIds[id].hubPlayer.nicknameSync.SetNick(name);
        }
        catch (Exception)
        {
            //Ignore
        }
        Plugin.plugin.FakeConnectionsIds[id].hubPlayer.roleManager.ServerSetRole(RoleTypeId.Overwatch, RoleChangeReason.RemoteAdmin);
    }
    public void OnRestartingRound()
    {
        foreach (var fakeConnectionList in Plugin.plugin.FakeConnectionsIds.Values)
        {
            fakeConnectionList.audioplayer.Stoptrack(true);
            fakeConnectionList.audioplayer.BroadcastChannel = VoiceChat.VoiceChatChannel.None;
        }
    }
    // Stole the code from the old AudioPlayer :jermasus:
    public void OnRoundStarted()
    {
        if (Plugin.plugin.LobbySong)
        {
            API.API.StopAudio();
            Plugin.plugin.LobbySong = false;
        }
        Plugin.plugin.Config.RoundStartClip.Play();
    }
    public void OnRoundEnded(RoundEndedEventArgs ev) => Plugin.plugin.Config.RoundEndClip.Play();
    public void OnRespawningTeam(RespawningTeamEventArgs ev)
    {
        switch (ev.NextKnownTeam)
        {
            case SpawnableTeamType.ChaosInsurgency:
                Plugin.plugin.Config.ChaosSpawnClip.Play();
                break;
            case SpawnableTeamType.NineTailedFox:
                Plugin.plugin.Config.MtfSpawnClip.Play();
                break;
        }
    }
}
