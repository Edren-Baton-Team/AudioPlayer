using Exiled.API.Features;
using HarmonyLib;
using Mirror;
using PlayerRoles;
using SCPSLAudioApi;
using SCPSLAudioApi.AudioCore;
using System;
using System.IO;
using VoiceChat;

namespace AudioPlayer
{
    public class Plugin : Plugin<Config>
    {
        public override string Prefix => "AudioPlayer";
        public override string Name => "AudioPlayer";
        public override string Author => "Rysik5318 and Mariki";
        public override System.Version Version { get; } = new System.Version(1, 0, 2);

        public static Plugin plugin;

        public FakeConnection fakeConnection;
        public ReferenceHub hubPlayer;
        public AudioPlayerBase audioplayer;
        public Harmony _harmonyInstance;
        public UnityEngine.GameObject newPlayer = UnityEngine.Object.Instantiate(NetworkManager.singleton.playerPrefab);
        public bool BotReady = false;

        public readonly string AudioPath = Path.Combine(Paths.Plugins, "Audio");

        public override void OnEnabled()
        {
            try
            {
                plugin = this;
                Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
                Startup.SetupDependencies();
                Extensions.CreateDirectory();
            }
            catch (Exception e)
            {
                Log.Error($"Error loading plugin: {e}");
            }
            try
            {
                _harmonyInstance = new Harmony($"Rysik5318.{DateTime.UtcNow.Ticks}");
                _harmonyInstance.PatchAll();
            }
            catch (Exception e)
            {
                Log.Error($"Error on patch harmony: {e.Data} -- {e.StackTrace}");
            }
            base.OnEnabled();
        }

        public void OnRoundStart()
        {
            BotReady = true;
            SpawnDummy(133, Config.BotName);
        }
        public static void SpawnDummy(int id, string name)
        {
            int idplayer = id;
            var hubPlayer = plugin.hubPlayer;
            var audioPlayer = plugin.audioplayer;
            plugin.fakeConnection = new FakeConnection(idplayer++);
            hubPlayer = plugin.newPlayer.GetComponent<ReferenceHub>();
            NetworkServer.AddPlayerForConnection(plugin.fakeConnection, plugin.newPlayer);
            hubPlayer.characterClassManager._privUserId = $"Audio-Player-{idplayer}@server";
            hubPlayer.characterClassManager.InstanceMode = ClientInstanceMode.Unverified;
            hubPlayer.characterClassManager.GodMode = true;
            hubPlayer.nicknameSync.SetNick(name);
            hubPlayer.roleManager.ServerSetRole(RoleTypeId.Overwatch, RoleChangeReason.RemoteAdmin);
            audioPlayer.BroadcastChannel = VoiceChatChannel.Intercom;
        }
        public override void OnDisabled()
        {
            plugin = this;
            base.OnDisabled();
        }
    }
}
