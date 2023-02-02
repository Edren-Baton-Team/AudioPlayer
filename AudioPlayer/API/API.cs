using SCPSLAudioApi.AudioCore;
using VoiceChat;

namespace AudioPlayer.API
{
    public class API
    {
        public static void PlayAudioFromFile(string path, float volume, VoiceChatChannel channel)
        {
            var audioPlayer = AudioPlayerBase.Get(Plugin.plugin.hubPlayer);
            audioPlayer.Volume = volume;
            audioPlayer.BroadcastChannel = channel;
            audioPlayer.Enqueue(path, -1);
            audioPlayer.Play(0);
        }
        public static void StopAudio()
        {
            var audioPlayer = AudioPlayerBase.Get(Plugin.plugin.hubPlayer);
            audioPlayer.Stoptrack(true);
        }
        public static void VolimeAudio(float volume)
        {
            var audioPlayer = AudioPlayerBase.Get(Plugin.plugin.hubPlayer);
            audioPlayer.Volume = volume;
        }
    }
}
