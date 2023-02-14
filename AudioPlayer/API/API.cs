using PlayerRoles.FirstPersonControl;
using SCPSLAudioApi.AudioCore;
using Exiled.API.Features;
using UnityEngine;
using VoiceChat;
using Mirror;
using System.Linq;
using HarmonyLib;

namespace AudioPlayer.API
{
    public class API
    {
        public FakeConnection fakeConnection;
        public ReferenceHub hubPlayer;
        public AudioPlayerBase audioplayer;
        public Harmony _harmonyInstance;
        public Object newPlayer = Object.Instantiate(NetworkManager.singleton.playerPrefab);
        public static API api;
        public static void PlayAudioFromFile(string path, float volume, VoiceChatChannel channel)
        {
            var audioPlayer = AudioPlayerBase.Get(api.hubPlayer);
            audioPlayer.Volume = volume;
            audioPlayer.BroadcastChannel = channel;
            audioPlayer.Enqueue(path, -1);
            audioPlayer.Play(0);
            Log.Debug("Bot AudioPlayer is Ready!");
        }
        public static void SpawnDummy()
        {
            var newPlayer = Object.Instantiate(NetworkManager.singleton.playerPrefab);
            int idplayer = Player.List.Count() + 1;
            api.fakeConnection = new FakeConnection(idplayer++);
            api.hubPlayer = newPlayer.GetComponent<ReferenceHub>();
            NetworkServer.AddPlayerForConnection(api.fakeConnection, newPlayer);
            api.hubPlayer.characterClassManager._privUserId = $"{idplayer}";
            api.hubPlayer.characterClassManager.InstanceMode = ClientInstanceMode.Unverified;
            api.hubPlayer.characterClassManager.GodMode = true;
            api.hubPlayer.TryOverridePosition(new Vector3(0, 0), new Vector3(0, 0));
            api.hubPlayer.roleManager.ServerSetRole(PlayerRoles.RoleTypeId.Overwatch, PlayerRoles.RoleChangeReason.RemoteAdmin);
            api.hubPlayer.nicknameSync.DisplayName = Plugin.plugin.Config.BotName;
            Log.Debug("Bot AudioPlayer is Ready!");
        }
        public static void StopAudio()
        {
            var audioPlayer = AudioPlayerBase.Get(api.hubPlayer);
            audioPlayer.Stoptrack(true);
            Log.Debug("AudioPlayer stopped playing sound");
        }
        public static void VolimeAudio(float volume)
        {
            var audioPlayer = AudioPlayerBase.Get(api.hubPlayer);
            audioPlayer.Volume = volume;
            Log.Debug($"AudioPlayer sound has been changed to - {volume}");
        }
        public static void DisconnectDummy()
        {
            NetworkServer.DestroyPlayerForConnection(api.fakeConnection);
            Log.Debug("AudioPlayer bot was kicked out of game!");
        }
    }
}
