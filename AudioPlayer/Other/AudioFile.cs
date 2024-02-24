using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.IO;
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
    public void Play()
    {
        Path = Extensions.PathCheck(Path);
        try
        {
            AudioController.PlayAudioFromFile(Path, Loop, Volume, VoiceChatChannel, id: BotId);
        }
        catch (Exception ex)
        {
            Log.Debug(ex.ToString());
        }
    }
    public void PlayFromFilePlayer(List<int> players)
    {
        Path = Extensions.PathCheck(Path);
        try
        {
            AudioController.PlayFromFilePlayer(players, Path, Loop, Volume, VoiceChatChannel, id: BotId);
        }
        catch (Exception ex)
        {
            Log.Debug(ex.ToString());
        }
    }
    public void Stop(bool lobbyPlaylist = false)
    {
        if (lobbyPlaylist) Plugin.plugin.LobbySong = null;
        AudioController.StopAudio(BotId);
    }
}