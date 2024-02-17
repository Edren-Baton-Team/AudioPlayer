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
        if (File.Exists(Path))
        {
            Log.Debug("The full path was specified, I'm skipping it");
        }
        else if (File.Exists(Plugin.plugin.AudioPath + "/" + Path))
        {
            Path = Plugin.plugin.AudioPath + "/" + Path;
            Log.Debug("An incomplete path was specified, I am looking for the .ogg file in the audio folder");
        }
        else
        {
            Log.Debug($"File not found on path {Path}");
            return;
        }

        AudioController.PlayAudioFromFile(Path, Loop, Volume, VoiceChatChannel, id: BotId);
    }
    public void PlayFromFilePlayer(List<int> players)
    {
        if (File.Exists(Path))
        {
            Log.Debug("The full path was specified, I'm skipping it");
        }
        else if (File.Exists(Plugin.plugin.AudioPath + "/" + Path))
        {
            Path = Plugin.plugin.AudioPath + "/" + Path;
            Log.Debug("An incomplete path was specified, I am looking for the .ogg file in the audio folder");
        }
        else
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
    public void Stop(bool lobbyPlaylist = false)
    {
        if (lobbyPlaylist) Plugin.plugin.LobbySong = null;
        AudioController.StopAudio(BotId);
    }
}