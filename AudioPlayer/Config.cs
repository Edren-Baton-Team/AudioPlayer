using AudioPlayer.Other;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Enums;

namespace AudioPlayer;

public class Config : IConfig
{
    [Description("Plugin Enabled?")]
    public bool IsEnabled { get; set; } = true;

    [Description("Enable developer mode? (AudioPlayer debug)")]
    public bool Debug { get; set; } = false;
    [Description("Command to use AudioPlayer")]
    public string[] CommandName { get; set; } = { "audio", "au" };
    [Description("Enable developer mode SCPSLAudioApi?")]
    public bool ScpslAudioApiDebug { get; set; } = false;
    [Description("Create a bot automatically when the server starts?")]
    public bool SpawnBot { get; set; } = true;

    [Description("The name of the bot when the file will sound. (Bot with ID 99 is better not to touch, he is responsible for commands on the server)")]
    public List<BotsList> BotsList { get; set; } = new() { new BotsList() { BotName = "Dedicated Server", BotId = 99 }, new BotsList() { BotName = "Elevator Audio", BotId = 98 } };

    [Description("Turn all special events on or off?")]
    public bool SpecialEventsEnable { get; set; } = false;

    [Description("Special Events Automatic Music, blank to disable.")]
    public List<AudioFile> LobbyPlaylist { get; set; } = new() { new AudioFile(), new AudioFile() { Path = System.IO.Path.Combine(Paths.Plugins, "audio", "test2.ogg") } };
    public List<AudioFile> RoundStartClip { get; set; } = new() { new AudioFile(), new AudioFile() { Path = System.IO.Path.Combine(Paths.Plugins, "audio", "test2.ogg") } };
    public List<AudioFile> RoundEndClip { get; set; } = new() { new AudioFile(), new AudioFile() { Path = System.IO.Path.Combine(Paths.Plugins, "audio", "test2.ogg") } };
    public List<AudioFile> MtfSpawnClip { get; set; } = new() { new AudioFile(), new AudioFile() { Path = System.IO.Path.Combine(Paths.Plugins, "audio", "test2.ogg") } };
    public List<AudioFile> ChaosSpawnClip { get; set; } = new() { new AudioFile(), new AudioFile() { Path = System.IO.Path.Combine(Paths.Plugins, "audio", "test2.ogg") } };
    public List<AudioFile> WarheadStartingClip { get; set; } = new() { new AudioFile(), new AudioFile() { Path = System.IO.Path.Combine(Paths.Plugins, "audio", "test2.ogg") } };

    [Description("Stop audio playback if the warhead has been disabled? (true = yes, false = no)")]
    public bool WarheadStopping = false;
    public List<AudioFile> WarheadStoppingClip { get; set; } = new() { new AudioFile(), new AudioFile() { Path = System.IO.Path.Combine(Paths.Plugins, "audio", "test2.ogg") } };
    public List<AudioFile> ElevatorClip { get; set; } = new() { new AudioFile(), new AudioFile() { Path = System.IO.Path.Combine(Paths.Plugins, "audio", "test2.ogg") } };
    public List<AudioFile> PlayerDiedTargetClip { get; set; } = new() { new AudioFile(), new AudioFile() { Path = System.IO.Path.Combine(Paths.Plugins, "audio", "test2.ogg") } };
    public List<AudioFile> PlayerDiedKillerClip { get; set; } = new() { new AudioFile(), new AudioFile() { Path = System.IO.Path.Combine(Paths.Plugins, "audio", "test2.ogg") } };
    public List<AudioFile> PlayerConnectedServer{ get; set; } = new() { new AudioFile(), new AudioFile() { Path = System.IO.Path.Combine(Paths.Plugins, "audio", "test2.ogg") } };
}
