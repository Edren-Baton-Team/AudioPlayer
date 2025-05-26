using System;
using System.Collections.Generic;
using AudioPlayer.API.Container;
using Exiled.API.Features;
using VoiceChat;
using AudioController = AudioPlayer.API.AudioController;

namespace AudioPlayer.Other;

public class AudioFile
{
    public AudioFile() { } // YamlDotNet Moment
    
    public AudioFile(string path, bool loop = false, int volume = 100, VoiceChatChannel voiceChatChannel = VoiceChatChannel.Intercom, int botId = 99)
    {
        Path = path;
        Loop = loop;
        Volume = volume;
        VoiceChatChannel = voiceChatChannel;
        BotId = botId;
    }
    
    public string Path { get; set; }
    public bool Loop { get; set; }
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