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
    public override Version Version => new Version(4, 1, 0);

    public static Dictionary<int, AudioPlayerBot> AudioPlayerList = [];

    public static Plugin plugin { get; private set; }

    internal static EventHandler EventHandlers;
    internal static SpecialEvents SpecialEvents;
    internal static WarheadEvents WarheadEvents;
    internal static LobbyEvents LobbyEvents;

    public readonly string AudioPath = Path.Combine(Paths.Plugins, "audio");

    public override void OnEnabled()
    {
        try
        {
            plugin = this;

            Extensions.EmptyClip = new AudioFile();
            Extensions.EmptyClip.BotId = -1;
            Extensions.EmptyClip.Path = "";

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

        plugin = null;

        base.OnDisabled();
    }
}
