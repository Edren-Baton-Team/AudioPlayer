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
    public int BotId { get; set; }
    public void Play(bool LobbyPlaylist = false)
    {
        if (!System.IO.File.Exists(Path))
        {
            Log.Debug($"File not found on path {Path}");
            return;
        }
        if (LobbyPlaylist)
            Plugin.plugin.LobbySong = true;

        AudioController.PlayAudioFromFile(Path, Loop, Volume, VoiceChatChannel, id: BotId);
    }
    public void PlayFromFilePlayer(List<int> players)
    {
        if (!System.IO.File.Exists(Path))
        {
            Log.Debug($"File not found on path {Path}");
            return;
        }
        try
        {
            AudioController.PlayFromFilePlayer(players, Path, Loop, Volume, VoiceChatChannel, id: BotId);
        }
        catch (Exception ex)
        {
            Log.Debug(ex.ToString());
        }
    }
    public void Stop()
    {
        AudioController.StopAudio(BotId);
    }
}