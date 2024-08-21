using AudioPlayer.Other;
using Exiled.API.Features;
using MEC;
using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VoiceChat;
using static AudioPlayer.Plugin;
using Extensions = AudioPlayer.Other.Extensions;

namespace AudioPlayer.API;
public static class AudioController
{
    public static void SpawnDummy(int id, string badgetext = "AudioPlayer BOT", string bagdecolor = "orange", string name = "Dedicated Server")
        => Extensions.SpawnDummy(name, badgetext, bagdecolor, id);
    public static void StopPlayerFromPlaying(List<int> players, int id = 99)
    {
        foreach (var player in players)
            Extensions.GetAudioBotFakeConnectionList(id).audioplayer.BroadcastTo.Remove(player);
    }
    public static void AddAudioEnqueue(string audio, int pos, int id = 99)
        => Extensions.GetAudioBotFakeConnectionList(id).audioplayer.Enqueue(audio, pos);
    public static void LogDebugAudio(bool logdebug = true, int id = 99)
        => Extensions.GetAudioBotFakeConnectionList(id).audioplayer.LogDebug = logdebug;
    public static void ContinueAudio(bool Continue = true, int id = 99)
        => Extensions.GetAudioBotFakeConnectionList(id).audioplayer.Continue = Continue;
    public static void ShuffleAudio(bool shuffle = false, int id = 99)
        => Extensions.GetAudioBotFakeConnectionList(id).audioplayer.Shuffle = shuffle;
    public static void LoopAudio(bool loop, int id = 99)
    => Extensions.GetAudioBotFakeConnectionList(id).audioplayer.Loop = loop;
    public static void VolumeAudio(float volume, int id = 99)
        => Extensions.GetAudioBotFakeConnectionList(id).audioplayer.Volume = volume;
    public static void PlayAudioFromFile(string path, bool loop = false, float volume = 100, VoiceChatChannel channel = VoiceChatChannel.Intercom, bool shuffle = false, bool logdebug = false, bool Continue = true, int id = 99)
    {
        if (!Extensions.TryGetAudioBot(id, out FakeConnectionList hub)) return;

        var audioPlayer = hub.audioplayer;
        audioPlayer.BroadcastChannel = channel;
        audioPlayer.Volume = volume;
        audioPlayer.Loop = loop;
        audioPlayer.Shuffle = shuffle;
        audioPlayer.Continue = Continue;
        audioPlayer.LogDebug = logdebug; //Welcome to Error spam ZONE!
        audioPlayer.Enqueue(path, -1);
        audioPlayer.Play(0);

    }
    public static void PlayFromFilePlayer(List<int> players, string path, bool loop = false, float volume = 100, VoiceChatChannel channel = VoiceChatChannel.Intercom, bool shuffle = false, bool logdebug = false, bool Continue = true, int id = 99)
    {
        if (!Extensions.TryGetAudioBot(id, out FakeConnectionList hub)) return;

        var audioPlayer = hub.audioplayer;
        audioPlayer.BroadcastTo = players;
        audioPlayer.BroadcastChannel = channel;
        audioPlayer.Volume = volume;
        audioPlayer.Loop = loop;
        audioPlayer.Shuffle = shuffle;
        audioPlayer.Continue = Continue;
        audioPlayer.LogDebug = logdebug;
        path = Extensions.PathCheck(path);
        audioPlayer.Enqueue(path, -1);
        audioPlayer.Play(0);

    }
    public static void StopAudio(int id = 99, bool clearAudioList = true)
    {
        if (Extensions.TryGetAudioBot(id, out FakeConnectionList hub))
        {
            hub.audioplayer.Stoptrack(clearAudioList);
            if (plugin.LobbySong != null) plugin.LobbySong = null;
        }
    }
    public static void DisconnectDummy(int id = 99)
    {
        if (Extensions.TryGetAudioBot(id, out FakeConnectionList hub))
        {
            if (hub.audioplayer.CurrentPlay != null)
            {
                hub.audioplayer.Stoptrack(true);
                hub.audioplayer.OnDestroy();
            }
            hub.hubPlayer.gameObject.transform.position = new Vector3(-9999f, -9999f, -9999f);
            Timing.CallDelayed(0.5f, () =>
            {
                NetworkServer.Destroy(hub.hubPlayer.gameObject);
                FakeConnectionsIds.Remove(id);
            });
        }
    }
}
