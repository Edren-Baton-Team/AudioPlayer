using Exiled.API.Features;
using System;
using System.Collections.Generic;
using VoiceChat;

namespace AudioPlayer;

public class AudioFile
{
    public string Path { get; set; } = System.IO.Path.Combine(Paths.Plugins, "audio", "test.ogg");
    public bool Loop { get; set; } = false;
    public int Volume { get; set; } = 100;
    public VoiceChatChannel VoiceChatChannel { get; set; } = VoiceChatChannel.Intercom;
    public int BotID { get; set; }
    public void Play(bool LobbyPlaylist = false)
    {
        if (!System.IO.File.Exists(Path))
        {
            Log.Debug($"File not found on path {Path}");
            return;
        }
        if (LobbyPlaylist)
            Plugin.plugin.LobbySong = true;

        API.AudioController.PlayAudioFromFile(Path, Loop, Volume, VoiceChatChannel, id: BotID);
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
            API.AudioController.PlayFromFilePlayer(players, Path, Loop, Volume, VoiceChatChannel, id: BotID);
        }
        catch (Exception ex)
        {
            Log.Debug(ex.ToString());
        }
    }
    public void Stop()
    {
        API.AudioController.StopAudio(BotID);
    }
}