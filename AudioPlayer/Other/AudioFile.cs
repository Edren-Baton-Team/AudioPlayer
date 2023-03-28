using Exiled.API.Features;

namespace AudioPlayer;

public class AudioFile
{
    public string Path { get; set; } = System.IO.Path.Combine(Paths.Plugins, "audio", "test.ogg");
    public int Volume { get; set; } = 100;
    public void Play(string text = "Play")
    {
        if (!System.IO.File.Exists(Path))
        {
            Log.Debug($"File not found on path {Path}");
            return;
        }
        if (text == "LobbyPlaylist")
            Plugin.plugin.LobbySong = true;

        API.API.PlayAudioFromFile(Path, false, Volume);
    }
}