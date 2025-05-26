using AudioPlayer.API.Container;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using VoiceChat;
using AudioController = AudioPlayer.API.AudioController;

namespace AudioPlayer.Other;

public class AudioFile(string path, bool loop = false, int volume = 100, VoiceChatChannel voiceChatChannel = VoiceChatChannel.Intercom, int botId = 99)
{
    public string Path { get; set; } = path;
    public bool Loop { get; set; } = loop;
    public int Volume { get; set; } = volume;
    public VoiceChatChannel VoiceChatChannel { get; set; } = voiceChatChannel;
    public int BotId { get; set; } = botId;
    AudioPlayerBot AudioPlayer => AudioController.TryGetAudioPlayerContainer(BotId);

    public void Play()
    {
        try
        {
            AudioPlayer.PlayAudioFromFile(Path, Loop, Volume, VoiceChatChannel);
        }
        catch (Exception ex)
        {
            Log.Debug(ex.ToString());
        }
    }

    public void PlayFromFilePlayer(List<int> players)
    {
        try
        {
            AudioPlayer.PlayFromFilePlayer(players, Path, Loop, Volume, VoiceChatChannel);
        }
        catch (Exception ex)
        {
            Log.Debug(ex.ToString());
        }
    }

    public void Stop()
    {
        try
        {
            AudioPlayer.StopAudio();
        }
        catch (Exception ex)
        {
            Log.Debug(ex.ToString());
        }
    }
}