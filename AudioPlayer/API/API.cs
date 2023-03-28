using SCPSLAudioApi.AudioCore;
using VoiceChat;
using Mirror;

namespace AudioPlayer.API
{
    public class API
    {
        public static void PlayAudioFromFile(string path, bool loop = false, float volume = 100, VoiceChatChannel channel = VoiceChatChannel.Intercom, int id = 99)
        {
            if (Plugin.plugin.FakeConnectionsIds.TryGetValue(id, out FakeConnectionList hub))
            {
                var audioPlayer = AudioPlayerBase.Get(hub.hubPlayer);
                audioPlayer.BroadcastChannel = channel;
                audioPlayer.Volume = volume;
                audioPlayer.Loop = loop;
                audioPlayer.Enqueue(path, -1);
                audioPlayer.Play(0);
            }
        }
        public static void SpawnDummy(int id, string name = "Dedicated Server")
        {
            Plugin.plugin.handlers.SpawnDummy(name, id);
        }
        public static void StopAudio(int id = 99)
        {
            if (Plugin.plugin.FakeConnectionsIds.TryGetValue(id, out FakeConnectionList hub))
            {
                var audioPlayer = AudioPlayerBase.Get(hub.hubPlayer);
                audioPlayer.Stoptrack(true);
                if (Plugin.plugin.LobbySong)
                    Plugin.plugin.LobbySong = false;
            }
        }
        public static void VolimeAudio(float volume, int id = 99)
        {
            if (Plugin.plugin.FakeConnectionsIds.TryGetValue(id, out FakeConnectionList hub))
            {
                var audioPlayer = AudioPlayerBase.Get(hub.hubPlayer);
                audioPlayer.Volume = volume;
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
