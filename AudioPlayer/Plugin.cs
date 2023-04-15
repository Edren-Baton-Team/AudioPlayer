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
    public override string Author => "Rysik5318 and Mariki";
    public override Version Version { get; } = new Version(2, 0, 5);
    public override Version RequiredExiledVersion { get; } = AutoUpdateExiledVersion.AutoUpdateExiledVersion.RequiredExiledVersion;

    public static Plugin plugin;

    public Dictionary<int, FakeConnectionList> FakeConnectionsIds = new Dictionary<int, FakeConnectionList>(); // It's more convenient.
    public EventHandler handlers;
    public SpecialEvents specialEvents;
    public bool LobbySong;
    public readonly string AudioPath = Path.Combine(Paths.Plugins, "audio");

    public override void OnEnabled()
    {
        try
        {
            plugin = this;
            FakeConnectionsIds.Clear();
            handlers = new EventHandler();
            Exiled.Events.Handlers.Server.WaitingForPlayers += handlers.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RestartingRound += handlers.OnRestartingRound;
            Exiled.Events.Handlers.Server.RoundStarted += handlers.OnRoundStarted;

            //SpecialEvents
            if (Config.SpecialEventsEnable)
            {
                specialEvents = new SpecialEvents();
                Exiled.Events.Handlers.Server.RoundStarted += specialEvents.OnRoundStarted;
                Exiled.Events.Handlers.Server.RoundEnded += specialEvents.OnRoundEnded;
                Exiled.Events.Handlers.Server.RespawningTeam += specialEvents.OnRespawningTeam;
                Exiled.Events.Handlers.Warhead.Starting += specialEvents.OnWarheadStarting;
                Exiled.Events.Handlers.Warhead.Stopping += specialEvents.OnWarheadStopping;
                Exiled.Events.Handlers.Player.Verified += specialEvents.OnVerified;
                Exiled.Events.Handlers.Player.Died += specialEvents.OnDied;
            }
            //

            //AudioEvents
            SCPSLAudioApi.AudioCore.AudioPlayerBase.OnFinishedTrack += handlers.OnFinishedTrack;
            CharacterClassManager.OnInstanceModeChanged += handlers.HandleInstanceModeChange;
            if (Config.ScpslAudioApiDebug)
            {
                SCPSLAudioApi.AudioCore.AudioPlayerBase.OnTrackSelecting += handlers.OnTrackSelecting;
                SCPSLAudioApi.AudioCore.AudioPlayerBase.OnTrackSelected += handlers.OnTrackSelected;
                SCPSLAudioApi.AudioCore.AudioPlayerBase.OnTrackLoaded += handlers.OnTrackLoaded;
                SCPSLAudioApi.AudioCore.AudioPlayerBase.OnFinishedTrack += handlers.OnFinishedTrackLog;
                Log.Warn($"SCPSLAudioApi Debug Enable!");
            }
            //

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
        handlers = null;
        Exiled.Events.Handlers.Server.RestartingRound -= handlers.OnRestartingRound;
        Exiled.Events.Handlers.Server.WaitingForPlayers -= handlers.OnWaitingForPlayers;
        Exiled.Events.Handlers.Server.RoundStarted -= handlers.OnRoundStarted;

        specialEvents = null;
        Exiled.Events.Handlers.Server.RoundStarted -= specialEvents.OnRoundStarted;
        Exiled.Events.Handlers.Server.RoundEnded -= specialEvents.OnRoundEnded;
        Exiled.Events.Handlers.Server.RespawningTeam -= specialEvents.OnRespawningTeam;
        Exiled.Events.Handlers.Warhead.Starting -= specialEvents.OnWarheadStarting;
        Exiled.Events.Handlers.Warhead.Stopping -= specialEvents.OnWarheadStopping;
        Exiled.Events.Handlers.Player.Verified -= specialEvents.OnVerified;
        Exiled.Events.Handlers.Player.Died -= specialEvents.OnDied;

        SCPSLAudioApi.AudioCore.AudioPlayerBase.OnFinishedTrack -= handlers.OnFinishedTrack;
        CharacterClassManager.OnInstanceModeChanged -= handlers.HandleInstanceModeChange;

        SCPSLAudioApi.AudioCore.AudioPlayerBase.OnTrackSelecting -= handlers.OnTrackSelecting;
        SCPSLAudioApi.AudioCore.AudioPlayerBase.OnTrackSelected -= handlers.OnTrackSelected;
        SCPSLAudioApi.AudioCore.AudioPlayerBase.OnTrackLoaded -= handlers.OnTrackLoaded;
        SCPSLAudioApi.AudioCore.AudioPlayerBase.OnFinishedTrack -= handlers.OnFinishedTrackLog;

        base.OnDisabled();
    }
}
