using Exiled.API.Features;
using HarmonyLib;
using Mirror;
using PlayerRoles;
using SCPSLAudioApi;
using SCPSLAudioApi.AudioCore;
using System;
using System.IO;

namespace AudioPlayer
{
    public class Plugin : Plugin<Config>
    {
        public override string Prefix => "AudioPlayer";
        public override string Name => "AudioPlayer";
        public override string Author => "Rysik5318 and Mariki";
        public override System.Version Version { get; } = new System.Version(1, 0, 1);

        public static Plugin plugin;

        public FakeConnection fakeConnection;
        public ReferenceHub hubPlayer;
        public AudioPlayerBase audioplayer;
        public Harmony _harmonyInstance;

        public readonly string AudioPath = Path.Combine($"{Paths.Plugins}", "Audio");

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
            SpawnDummy(133, Config.BotName);

        }
        public static void SpawnDummy(int id, string name)
        {
            var newPlayer = UnityEngine.Object.Instantiate(NetworkManager.singleton.playerPrefab);
            int idplayer = id;
            var hubPlayer = plugin.hubPlayer;
            var fakeConnection = plugin.fakeConnection;
            fakeConnection = new FakeConnection(idplayer++);
            hubPlayer = newPlayer.GetComponent<ReferenceHub>();
            NetworkServer.AddPlayerForConnection(fakeConnection, newPlayer);
            hubPlayer.characterClassManager._privUserId = $"Audio-Player-{idplayer}@server";
            hubPlayer.characterClassManager.InstanceMode = ClientInstanceMode.Unverified;
            hubPlayer.characterClassManager.GodMode = true;
            hubPlayer.nicknameSync.SetNick(name);
            hubPlayer.roleManager.ServerSetRole(RoleTypeId.Overwatch, RoleChangeReason.RemoteAdmin);
        }
        public override void OnDisabled()
        {
            plugin = this;
            base.OnDisabled();
        }
    }
}
