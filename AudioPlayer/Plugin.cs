using AudioPlayer.API.Container;
using AudioPlayer.Other;
using AudioPlayer.Other.DLC;
using Exiled.API.Features;
using SCPSLAudioApi;
using System;
using System.Collections.Generic;
using System.IO;

namespace AudioPlayer;

public class Plugin : Plugin<Config>
{
    public override string Prefix => "AudioPlayer";
    public override string Name => "AudioPlayer";
    public override string Author => "Rysik5318 & Mariki";
    public override Version Version => new Version(4, 0, 0);

    public static Dictionary<int, AudioPlayerBot> AudioPlayerList = [];

    public static Plugin plugin;
    internal EventHandler EventHandlers;
    internal SpecialEvents SpecialEvents;
    public AudioFile LobbySong = null;
    public readonly string AudioPath = Path.Combine(Paths.Plugins, "audio");

    public override void OnEnabled()
    {
        try
        {
            plugin = this;
            EventHandlers = new();
            if (Config.SpecialEventsEnable)
            {
                SpecialEvents = new();
            }
            Startup.SetupDependencies();
            Extensions.CreateDirectory();
        }
        catch (Exception e)
        {
            Log.Error($"Error loading plugin: {e}");
        }
        base.OnEnabled();
    }
    public override void OnDisabled()
    {
        EventHandlers = null;
        SpecialEvents = null;

        plugin = null;

        base.OnDisabled();
    }
}
