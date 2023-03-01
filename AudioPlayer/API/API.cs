using SCPSLAudioApi.AudioCore;
using VoiceChat;
using Mirror;
using MEC;
using System;
using PlayerRoles;
using System.Linq;

namespace AudioPlayer.API
{
    public class API
    {
        public static void PlayAudioFromFile(int id, string path, float volume = 100, VoiceChatChannel channel = VoiceChatChannel.Intercom)
        {
            if (Plugin.plugin.FakeConnectionsIds.TryGetValue(id, out ReferenceHub hub))
            {
                var audioPlayer = AudioPlayerBase.Get(hub);
                audioPlayer.BroadcastChannel = channel;
                audioPlayer.Volume = volume;
                audioPlayer.Enqueue(path, -1);
                audioPlayer.Play(0);
            }
        }
        public static void SpawnDummy(int id, string name = "Intercom")
        {
            var newPlayer = UnityEngine.Object.Instantiate(NetworkManager.singleton.playerPrefab);
            var fakeConnection = new FakeConnection(id);
            var hubPlayer = newPlayer.GetComponent<ReferenceHub>();
            Plugin.plugin.FakeConnections.Add(fakeConnection, hubPlayer);
            Plugin.plugin.FakeConnectionsIds.Add(id, hubPlayer);

            NetworkServer.AddPlayerForConnection(fakeConnection, newPlayer);
            try
            {
                hubPlayer.characterClassManager.UserId = $"player{id}@server";
            }
            catch (Exception)
            {
            }
            hubPlayer.characterClassManager.InstanceMode = ClientInstanceMode.Host;
            try
            {
                hubPlayer.nicknameSync.SetNick(name);
            }
            catch (Exception)
            {
            }
            Timing.CallDelayed(0.1f, () =>
            {
                hubPlayer.roleManager.ServerSetRole(RoleTypeId.CustomRole, RoleChangeReason.RemoteAdmin);
            });
        }
        public static void StopAudio(int id)
        {
            if (Plugin.plugin.FakeConnectionsIds.TryGetValue(id, out ReferenceHub hub))
            {
                var audioPlayer = AudioPlayerBase.Get(hub);
                audioPlayer.Stoptrack(true);
            }
        }
        public static void VolimeAudio(int id, float volume)
        {
            if (Plugin.plugin.FakeConnectionsIds.TryGetValue(id, out ReferenceHub hub))
            {
                var audioPlayer = AudioPlayerBase.Get(hub);
                audioPlayer.Volume = volume;
            }
        }
        public static void DisconnectDummy(int id)
        {
            if (Plugin.plugin.FakeConnectionsIds.TryGetValue(id, out ReferenceHub hub))
            {
                Plugin.plugin.FakeConnections.Remove(Plugin.plugin.FakeConnections.FirstOrDefault(s => s.Value == hub).Key);
                Plugin.plugin.FakeConnectionsIds.Remove(id);
                NetworkServer.Destroy(hub.gameObject);
            }
        }
    }
}
