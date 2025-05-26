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
    public static readonly Dictionary<int, AudioPlayerBot> AudioPlayerList = [];
    public readonly string AudioPath = Path.Combine(Paths.Plugins, "audio");
    public override string Prefix => "AudioPlayer";
    public override string Name => "AudioPlayer";
    public override string Author => "Rysik5318 & Mariki";
    public override Version Version => new(5, 0, 0);

    public static Plugin Instance { get; private set; }

    internal static EventHandler EventHandlers;
    internal static SpecialEvents SpecialEvents;
    internal static WarheadEvents WarheadEvents;
    internal static LobbyEvents LobbyEvents;

    public override void OnEnabled()
    {
        try
        {
            Instance = this;

            EventHandlers = new();

            if (Config.SpecialEventsEnable)
            {
                SpecialEvents = new();
                WarheadEvents = new();
                LobbyEvents = new();
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
        WarheadEvents = null;
        LobbyEvents = null;

        Instance = null;

        base.OnDisabled();
    }
}