using Exiled.API.Features;
using Exiled.API.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;

namespace AudioPlayer;

public class Config : IConfig
{
    [Description("Plugin Enabled?")]
    public bool IsEnabled { get; set; } = true;

    [Description("Enable developer mode?")]
    public bool Debug { get; set; } = true; 
    [Description("Create a bot automatically when the server starts?")]
    public bool SpawnBot { get; set; } = true;

    [Description("The name of the bot when the file will sound")]
    public string BotName { get; set; } = "Dedicated Server";

    [Description("Special Events Automatic Music, blank to disable.")]
    public List<AudioFile> LobbyPlaylist { get; set; } = new() { new AudioFile(), new AudioFile() { Path = System.IO.Path.Combine(Paths.Plugins, "audio", "test2.ogg") } };
    public List<AudioFile> RoundStartClip { get; set; } = new() { new AudioFile(), new AudioFile() { Path = System.IO.Path.Combine(Paths.Plugins, "audio", "test2.ogg") } };
    public List<AudioFile> RoundEndClip { get; set; } = new() { new AudioFile(), new AudioFile() { Path = System.IO.Path.Combine(Paths.Plugins, "audio", "test2.ogg") } };
    public List<AudioFile> MtfSpawnClip { get; set; } = new() { new AudioFile(), new AudioFile() { Path = System.IO.Path.Combine(Paths.Plugins, "audio", "test2.ogg") } };
    public List<AudioFile> ChaosSpawnClip { get; set; } = new() { new AudioFile(), new AudioFile() { Path = System.IO.Path.Combine(Paths.Plugins, "audio", "test2.ogg") } };
    public List<AudioFile> WarheadStartingClip { get; set; } = new() { new AudioFile(), new AudioFile() { Path = System.IO.Path.Combine(Paths.Plugins, "audio", "test2.ogg") } };
    public List<AudioFile> WarheadStoppingClip { get; set; } = new() { new AudioFile(), new AudioFile() { Path = System.IO.Path.Combine(Paths.Plugins, "audio", "test2.ogg") } };
}
