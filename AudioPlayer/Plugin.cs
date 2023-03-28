using System.Collections.Generic;
using Exiled.API.Features;
using SCPSLAudioApi;
using System.IO;
using System;
using AudioPlayer.API;

namespace AudioPlayer;

public class Plugin : Plugin<Config>
{
    public override string Prefix => "AudioPlayer";
    public override string Name => "AudioPlayer";
    public override string Author => "Rysik5318 and Mariki";
    public override Version Version { get; } = new Version(1, 0, 7);

    public static Plugin plugin;

    public Dictionary<int, FakeConnectionList> FakeConnectionsIds = new Dictionary<int, FakeConnectionList>(); // It's more convenient.
    public EventHandler handlers;
    public bool LobbySong;
    public readonly string AudioPath = Path.Combine(Paths.Plugins, "audio");

    public override void OnEnabled()
    {
        try
        {
            plugin = this;
            LobbySong = false;
            handlers = new EventHandler();
            Exiled.Events.Handlers.Server.WaitingForPlayers += handlers.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted += handlers.OnRoundStarted;
            Exiled.Events.Handlers.Server.RoundEnded += handlers.OnRoundEnded;
            Exiled.Events.Handlers.Server.RespawningTeam += handlers.OnRespawningTeam;
            Exiled.Events.Handlers.Server.RestartingRound += handlers.OnRestartingRound;
            Startup.SetupDependencies();
            Extensions.CreateDirectory();
            Log.Info("Loading AudioPlayer Event...");
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
        handlers = new EventHandler();
        Exiled.Events.Handlers.Server.RoundStarted -= handlers.OnRoundStarted;
        Exiled.Events.Handlers.Server.RoundEnded -= handlers.OnRoundEnded;
        Exiled.Events.Handlers.Server.RespawningTeam -= handlers.OnRespawningTeam;
        Exiled.Events.Handlers.Server.WaitingForPlayers -= handlers.OnWaitingForPlayers;
        Exiled.Events.Handlers.Server.RestartingRound -= handlers.OnRestartingRound;
        base.OnDisabled();
    }
}
