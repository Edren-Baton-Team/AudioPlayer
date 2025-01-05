using AudioPlayer.API.Container;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using VoiceChat;
using AudioController = AudioPlayer.API.AudioController;

namespace AudioPlayer.Other;

public class AudioFile
{
    public string Path { get; set; } = System.IO.Path.Combine(Paths.Plugins, "audio", "test.ogg");
    public bool Loop { get; set; } = false;
    public int Volume { get; set; } = 100;
    public VoiceChatChannel VoiceChatChannel { get; set; } = VoiceChatChannel.Intercom;
    public int BotId { get; set; } = 99;
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