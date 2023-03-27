using Exiled.API.Features;
using SCPSLAudioApi;
using HarmonyLib;
using System.IO;
using System;
using SCPSLAudioApi.AudioCore;
using System.Collections.Generic;
using System.Linq;

namespace AudioPlayer
{
    public class Plugin : Plugin<Config>
    {
        public override string Prefix => "AudioPlayer";
        public override string Name => "AudioPlayer";
        public override string Author => "Rysik5318 and Mariki";
        public override Version Version { get; } = new Version(1, 0, 6);

        public static Plugin plugin;

        public Dictionary<FakeConnection, ReferenceHub> FakeConnections = new Dictionary<FakeConnection, ReferenceHub>();
        public Dictionary<int, ReferenceHub> FakeConnectionsIds = new Dictionary<int, ReferenceHub>();
        public EventHandler handlers;
        public static FakeConnection fakeConnection;
        public static ReferenceHub hubPlayer;
        public static AudioPlayerBase audioplayer;

        public readonly string AudioPath = Path.Combine(Paths.Plugins, "audio");

        public override void OnEnabled()
        {
            try
            {
                plugin = this;
                handlers = new EventHandler();
                Exiled.Events.Handlers.Server.WaitingForPlayers += handlers.OnWaitingForPlayers;
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
            plugin = this;
            handlers = new EventHandler();
            Exiled.Events.Handlers.Server.WaitingForPlayers -= handlers.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RestartingRound -= handlers.OnRestartingRound;
            base.OnDisabled();
        }
    }
}
