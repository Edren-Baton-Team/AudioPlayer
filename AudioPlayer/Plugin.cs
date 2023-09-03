using AudioPlayer.Other.DLC;
using Exiled.API.Features;
using SCPSLAudioApi;
using System;
using System.Collections.Generic;
using System.IO;
using AudioPlayer.Other;

namespace AudioPlayer;

public class Plugin : Plugin<Config>
{
    public override string Prefix => "AudioPlayer";
    public override string Name => "AudioPlayer";
    public override string Author => "Rysik5318 and Mariki";
    public override Version Version { get; } = new Version(3, 0, 0);
    public static Plugin plugin; //troll 

    public static Dictionary<int, FakeConnectionList> FakeConnectionsIds = new(); // It's more convenient.
    internal EventHandler EventHandlers;
    internal SpecialEvents specialEvents;
    public bool LobbySong;
    public readonly string AudioPath = Path.Combine(Paths.Plugins, "audio");

    public override void OnEnabled()
    {
        try
        {
            plugin = this;
            EventHandlers = new EventHandler();
            if (Config.SpecialEventsEnable)
            {
                specialEvents = new SpecialEvents();
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
        plugin = null;
        EventHandlers = null;
        specialEvents = null;

        base.OnDisabled();
    }
}
