using Exiled.Permissions.Extensions;
using System.Collections.Generic;
using CommandSystem;
using PlayerRoles;
using RemoteAdmin;
using System.Linq;
using System.IO;
using VoiceChat;
using Mirror;
using System;
using MEC;
using SCPSLAudioApi.AudioCore;

namespace AudioPlayer
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class AudioCommand : ICommand
    {
        public static Dictionary<FakeConnection, ReferenceHub> FakeConnections = new Dictionary<FakeConnection, ReferenceHub>();
        public static Dictionary<int, ReferenceHub> FakeConnectionsIds = new Dictionary<int, ReferenceHub>();

        public string Command { get; } = "audio";

        public string[] Aliases { get; } = { "au" };

        public string Description { get; } = "Audio Command";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender is PlayerCommandSender raSender))
            {
                response = "Console cant execute this command!";
                return false;
            }

            if (!sender.CheckPermission("audioplayer.audio"))
            {
                response = "You dont have permission to run this command.";
                return false;
            }

            AudioPlayerBase player = AudioPlayerBase.Get(raSender.ReferenceHub);

            if (arguments.Count == 0)
            {
                response = "" +
                    "\nArgs:" +
                    "\naudio add {playerid} - spawns a dummyplayer" +
                    "\naudio destroy {playerid} - Destroys the player" +
                    "\naudio play {playerid} {index} - starts playing at the given index" +
                    "\naudio stop {playerid} - Stop music" +
                    "\naudio volume {playerid} {volume} - sets the volume" +
                    "\naudio channel {playerid} {VoiceChatChannel} - Changes bot VoiceChannel" +
                    "\naudio nick {playerid} {Name} - Sets name of bot";
                return false;
            }

            switch (arguments.At(0).ToLower())
            {
                case "add":
                case "spawn":
                case "create":
                case "fake":
                    {
                        int id = int.Parse(arguments.At(1));

                        var newPlayer = UnityEngine.Object.Instantiate(NetworkManager.singleton.playerPrefab);

                        var fakeConnection = new FakeConnection(id);


                        var hubPlayer = newPlayer.GetComponent<ReferenceHub>();
                        FakeConnections.Add(fakeConnection, hubPlayer);
                        FakeConnectionsIds.Add(id, hubPlayer);

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
                            hubPlayer.nicknameSync.SetNick($"Dummy player {id}");
                        }
                        catch (Exception)
                        {
                        }
                        Timing.CallDelayed(0.1f, () =>
                        {
                            hubPlayer.roleManager.ServerSetRole(RoleTypeId.CustomRole, RoleChangeReason.RemoteAdmin);
                        });
                    }
                    break;
                case "setnick":
                case "nick":
                case "name":
                case "n":
                    {
                        if (arguments.Count == 1 || arguments.Count == 2)
                        {
                            response = "Usage: audio nick {ID} {Name}";
                            return false;
                        }
                        int id = int.Parse(arguments.At(1));
                        if (FakeConnectionsIds.TryGetValue(id, out ReferenceHub hub))
                        {
                            hub.nicknameSync.SetNick(arguments.At(2));
                        }
                    }
                    break;
                case "p":
                case "play":
                    {
                        if (arguments.Count == 1 || arguments.Count == 2)
                        {
                            response = "Usage: audio play {ID} {path}";
                            return false;
                        }
                        int id = int.Parse(arguments.At(1));
                        if (FakeConnectionsIds.TryGetValue(id, out ReferenceHub hub))
                        {
                            var audioPlayer = AudioPlayerBase.Get(hub);
                            string path = arguments.At(2);

                            if (!File.Exists(path))
                            {
                                response = $"No files exist inside that path.\nPath: {path}";
                                return false;
                            }

                            audioPlayer.Enqueue(path, -1);
                            audioPlayer.Play(0);
                        }
                    }
                    break;
                case "s":
                case "stop":
                    {
                        if (!sender.CheckPermission("audioplayer.stop"))
                        {
                            response = "You dont have perms to do that";
                            return false;
                        }
                        if (arguments.Count == 1)
                        {
                            response = "Usage: audio stop {ID}";
                            return false;
                        }
                        int id = int.Parse(arguments.At(1));
                        if (FakeConnectionsIds.TryGetValue(id, out ReferenceHub hub))
                        {
                            var audioPlayer = AudioPlayerBase.Get(hub);
                            audioPlayer.Stoptrack(true);
                        }
                    }
                    break;
                case "kick":
                case "destroy":
                    {
                        if (!sender.CheckPermission("audioplayer.kickbot"))
                        {
                            response = "You dont have perms to do that";
                            return false;
                        }
                        if (arguments.Count == 1)
                        {
                            response = "Usage: audio kick {ID}";
                            return false;
                        }
                        int id = int.Parse(arguments.At(1));
                        if (FakeConnectionsIds.TryGetValue(id, out ReferenceHub hub))
                        {
                            FakeConnections.Remove(FakeConnections.FirstOrDefault(s => s.Value == hub).Key);
                            FakeConnectionsIds.Remove(id);
                            NetworkServer.Destroy(hub.gameObject);
                        }
                    }
                    break;
                case "v":
                case "vol":
                case "volume":
                    {
                        if (!sender.CheckPermission("audioplayer.volume"))
                        {
                            response = "You dont have perms to do that";
                            return false;
                        }
                        if (arguments.Count == 1 || arguments.Count == 2)
                        {
                            response = "Usage: audio volume {ID} {Number}";
                            return false;
                        }
                        int id = int.Parse(arguments.At(1));
                        if (FakeConnectionsIds.TryGetValue(id, out ReferenceHub hub))
                        {
                            var audioPlayer = AudioPlayerBase.Get(hub);
                            audioPlayer.Volume = Convert.ToInt32(arguments.At(2));
                        }
                    }
                    break;
                case "chan":
                case "channel":
                case "audiochannel":
                    {
                        if (!sender.CheckPermission("audioplayer.channel"))
                        {
                            response = "You dont have perms to do that";
                            return false;
                        }
                        if (arguments.Count == 1 || arguments.Count == 2)
                        {
                            response = "Usage: audio channel {ID} {VoiceChatChannel}";
                            return false;
                        }

                        int id = int.Parse(arguments.At(1));
                        if (FakeConnectionsIds.TryGetValue(id, out ReferenceHub hub))
                        {
                            var audioPlayer = AudioPlayerBase.Get(hub);
                            audioPlayer.BroadcastChannel = (VoiceChatChannel)Enum.Parse(typeof(VoiceChatChannel), arguments.At(2));
                        }
                    }
                    break;
            }
            response = "Done";
            return true;
        }
    }
}