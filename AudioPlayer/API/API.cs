using SCPSLAudioApi.AudioCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceChat;

namespace AudioPlayer.API
{
    public class API
    {
        public static void PlayAudioFromFile(string path,float volume,VoiceChatChannel channel)
        {
            var audioPlayer = AudioPlayerBase.Get(Plugin.plugin.hubPlayer);
            audioPlayer.Volume= volume;
            audioPlayer.BroadcastChannel = channel;
            audioPlayer.Enqueue(path, -1);
            audioPlayer.Volume = 100;
            audioPlayer.Play(0);
        }
    }
}
