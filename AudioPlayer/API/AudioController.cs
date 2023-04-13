using VoiceChat;
using Mirror;
using System.Collections.Generic;

namespace AudioPlayer.API
{
    public class AudioController
    {
        public static void PlayAudioFromFile(string path, bool loop = false, float volume = 100, VoiceChatChannel channel = VoiceChatChannel.Intercom, bool shuffle = false, bool logdebug = false, bool Continue = true, int id = 99)
        {
            if (Plugin.plugin.FakeConnectionsIds.TryGetValue(id, out FakeConnectionList hub))
            {
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
        }
        public static void SpawnDummy(int id, bool showplayer = false, string badgetext = "AudioPlayer BOT", string bagdecolor = "orange", string name = "Dedicated Server")
        {
            Plugin.plugin.handlers.SpawnDummy(name, showplayer, badgetext, bagdecolor, id);
        }
        public static void StopPlayerFromPlaying(List<int> players, int id = 99)
        {
            if (Plugin.plugin.FakeConnectionsIds.TryGetValue(id, out FakeConnectionList hub))
            {
                foreach (var ply in players)
                {
                    hub.audioplayer.BroadcastTo.Remove(ply);
                }
            }
        }
        public static void PlayFromFilePlayer(List<int> players, string path, bool loop = false, float volume = 100, VoiceChatChannel channel = VoiceChatChannel.Intercom, bool shuffle = false, bool logdebug = false, bool Continue = true, int id = 99)
        {
            if (Plugin.plugin.FakeConnectionsIds.TryGetValue(id, out FakeConnectionList hub))
            {
                var audioPlayer = hub.audioplayer;
                audioPlayer.BroadcastTo = players;
                audioPlayer.BroadcastChannel = channel;
                audioPlayer.Volume = volume;
                audioPlayer.Loop = loop;
                audioPlayer.Shuffle = shuffle;
                audioPlayer.Continue = Continue;
                audioPlayer.LogDebug = logdebug;
                audioPlayer.Enqueue(path, -1);
                audioPlayer.Play(0);
            }
        }
        public static void AddAudioEnqueue(string audio, int pos, int id = 99)
        {
            if (Plugin.plugin.FakeConnectionsIds.TryGetValue(id, out FakeConnectionList hub))
            {
                hub.audioplayer.Enqueue(audio, pos);
            }
        }
        public static void LogDebugAudio(bool logdebug = true, int id = 99)
        {
            if (Plugin.plugin.FakeConnectionsIds.TryGetValue(id, out FakeConnectionList hub))
            {
                hub.audioplayer.LogDebug = logdebug;
            }
        }
        public static void ContinueAudio(bool Continue = true, int id = 99) //Whether the Player should continue playing by itself after the current Track ends?
        {
            if (Plugin.plugin.FakeConnectionsIds.TryGetValue(id, out FakeConnectionList hub))
            {
                hub.audioplayer.Continue = Continue;
            }
        }
        public static void ShuffleAudio(bool shuffle = false, int id = 99)
        {
            if (Plugin.plugin.FakeConnectionsIds.TryGetValue(id, out FakeConnectionList hub))
            {
                hub.audioplayer.Shuffle = shuffle;
            }
        }
        public static void StopAudio(int id = 99)
        {
            if (Plugin.plugin.FakeConnectionsIds.TryGetValue(id, out FakeConnectionList hub))
            {
                hub.audioplayer.Stoptrack(true);
                if (Plugin.plugin.LobbySong)
                    Plugin.plugin.LobbySong = false;
            }
        }
        public static void LoopAudio(bool loop, int id = 99)
        {
            if (Plugin.plugin.FakeConnectionsIds.TryGetValue(id, out FakeConnectionList hub))
            {
                hub.audioplayer.Loop = loop;
            }
        }
        public static void VolimeAudio(float volume, int id = 99)
        {
            if (Plugin.plugin.FakeConnectionsIds.TryGetValue(id, out FakeConnectionList hub))
            {
                hub.audioplayer.Volume = volume;
            }
        }
        public static void DisconnectDummy(int id)
        {
            if (Plugin.plugin.FakeConnectionsIds.TryGetValue(id, out FakeConnectionList hub))
            {
                Plugin.plugin.FakeConnectionsIds.Remove(id);    
                NetworkServer.Destroy(hub.hubPlayer.gameObject);
            }
        }
    }
}
