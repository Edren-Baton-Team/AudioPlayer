using Exiled.API.Features;
using SCPSLAudioApi;
using HarmonyLib;
using System.IO;
using System;
using SCPSLAudioApi.AudioCore;
using System.Collections.Generic;

namespace AudioPlayer
{
    public class Plugin : Plugin<Config>
    {
        public override string Prefix => "AudioPlayer";
        public override string Name => "AudioPlayer";
        public override string Author => "Rysik5318 and Mariki";
        public override Version Version { get; } = new System.Version(1, 0, 3);

        public static Plugin plugin;

        public Dictionary<FakeConnection, ReferenceHub> FakeConnections = new Dictionary<FakeConnection, ReferenceHub>();
        public Dictionary<int, ReferenceHub> FakeConnectionsIds = new Dictionary<int, ReferenceHub>();

        public FakeConnection fakeConnection;
        public ReferenceHub hubPlayer;
        public AudioPlayerBase audioplayer;

        public Harmony _harmonyInstance;

        public readonly string AudioPath = Path.Combine(Paths.Plugins, "audio");

        public override void OnEnabled()
        {
            try
            {
                plugin = this;
                Startup.SetupDependencies();
                Extensions.CreateDirectory();
                Log.Info("Loading AudioPlayer Event...");
            }
            catch (Exception e)
            {
                Log.Error($"Error loading plugin: {e}");
            }
            try
            {
                _harmonyInstance = new Harmony($"Rysik5318.{DateTime.UtcNow.Ticks}");
                _harmonyInstance.PatchAll();
                Log.Info("Loading AudioPlayer Harmory Patch...");
            }
            catch (Exception e)
            {
                Log.Error($"Error on patch harmony: {e.Data} -- {e.StackTrace}");
            }
            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            plugin = this;
            base.OnDisabled();
        }
    }
}
