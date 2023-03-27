using Exiled.API.Features;
using Mirror;
using PlayerRoles;
using SCPSLAudioApi.AudioCore;
using System;

namespace AudioPlayer
{
    public class EventHandler
    {
        public void OnWaitingForPlayers()
        {
            Plugin.plugin.spawnbot = true;
            SpawnDummyStartRound();
        }
        public void SpawnDummy(string name, int id = 99)
        {
            var newPlayer = UnityEngine.Object.Instantiate(NetworkManager.singleton.playerPrefab);
            var fakeConnection = new FakeConnection(id);
            var hubPlayer = newPlayer.GetComponent<ReferenceHub>();
            if (Plugin.plugin.FakeConnectionsIds.ContainsKey(id))
            {
                Log.Error("This id is already in use");
            }
            Plugin.plugin.FakeConnections.Add(fakeConnection, hubPlayer);
            Plugin.plugin.FakeConnectionsIds.Add(id, hubPlayer);

            NetworkServer.AddPlayerForConnection(fakeConnection, newPlayer);
            try
            {
                Plugin.plugin.FakeConnectionsIds[id].characterClassManager.UserId = $"player{id}@server";
            }
            catch (Exception)
            {
                //Ignore
            }
            Plugin.plugin.FakeConnectionsIds[id].characterClassManager.InstanceMode = ClientInstanceMode.Host;
            try
            {
                Plugin.plugin.FakeConnectionsIds[id].nicknameSync.SetNick(name);
            }
            catch (Exception)
            {
                //Ignore
            }
            Plugin.plugin.FakeConnectionsIds[id].roleManager.ServerSetRole(RoleTypeId.Overwatch, RoleChangeReason.RemoteAdmin);
        }
        private void SpawnDummyStartRound()
        {
            var newPlayer = UnityEngine.Object.Instantiate(NetworkManager.singleton.playerPrefab);
            int id = 99;
            Plugin.fakeConnection = new FakeConnection(id);
            Plugin.hubPlayer = newPlayer.GetComponent<ReferenceHub>();
            Plugin.plugin.FakeConnections.Add(Plugin.fakeConnection, Plugin.hubPlayer);
            Plugin.plugin.FakeConnectionsIds.Add(id, Plugin.hubPlayer);
            Plugin.audioplayer = AudioPlayerBase.Get(Plugin.hubPlayer);

            NetworkServer.AddPlayerForConnection(Plugin.fakeConnection, newPlayer);
            try
            {
                Plugin.hubPlayer.characterClassManager.UserId = $"player{id}@server";
            }
            catch (Exception)
            {
                //Ignore
            }
            Plugin.hubPlayer.characterClassManager.InstanceMode = ClientInstanceMode.Host;
            try
            {
                Plugin.hubPlayer.nicknameSync.SetNick(Plugin.plugin.Config.BotName);
            }
            catch (Exception)
            {
                //Ignore
            }
            Plugin.hubPlayer.roleManager.ServerSetRole(RoleTypeId.Overwatch, RoleChangeReason.RemoteAdmin);
        }
        public void OnRestartingRound()
        {
            Plugin.audioplayer.Stoptrack(true);
            Plugin.audioplayer.BroadcastChannel = VoiceChat.VoiceChatChannel.None;
        }
    }
}
