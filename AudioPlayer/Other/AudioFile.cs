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
    private AudioPlayerBot audioPlayer => AudioController.TryGetAudioPlayerContainer(BotId);
    
    public void Play()
    {
        try
        {
            audioPlayer.PlayAudioFromFile(Path, Loop, Volume, VoiceChatChannel);
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
            audioPlayer.PlayFromFilePlayer(players, Path, Loop, Volume, VoiceChatChannel);
        }
        catch (Exception ex)
        {
            Log.Debug(ex.ToString());
        }
    }
    
    public void Stop(bool lobbyPlaylist = false)
    {
        if (lobbyPlaylist)
        {
            Plugin.plugin.LobbySong = null;
        }
        audioPlayer.StopAudio();
    }
}