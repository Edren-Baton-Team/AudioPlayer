using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using Mirror;
using SCPSLAudioApi.AudioCore;
using System;
using System.IO;
using VoiceChat;

namespace AudioPlayer.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Audio : ICommand, IUsageProvider
    {
        public string Command { get; } = "audio";

        public string Description { get; } = "Audio Player command";

        public string[] Aliases { get; } = { "au" };

        public string[] Usage { get; set; } = new string[] { "play/stop/volume/spawn/kick/channel/nickname" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            var audioPlayer = AudioPlayerBase.Get(Plugin.plugin.hubPlayer);
            var hubPlayer = Plugin.plugin.hubPlayer;
            var BotReady = Plugin.plugin.BotReady;
            var newPlayer = UnityEngine.Object.Instantiate(NetworkManager.singleton.playerPrefab);
            if (arguments.Count == 0)
            {
                response = "" +
                    "\nArgs: " +
                    "\naudio play {Path} - Playing Audio From Path" +
                    "\naudio stop - Stop music" +
                    "\naudio volume {volume} - Set AudioPlayer Volume" +
                    "\naudio spawn - Spawn AudioPlayer Bot" +
                    "\naudio destroy - Destroys AudioPlayer Bot" +
                    "\naudio channel {VoiceChatChannel} - Changes bot VoiceChannel" +
                    "\naudio nickname {Name} - Sets name of bot";
                return false;
            }
            switch (arguments.At(0).ToLower())
            {
                case "p":
                case "play":
                    {
                        if (!sender.CheckPermission("audioplayer.play"))
                        {
                            response = "You dont have perms to do that";
                            return false;
                        }
                        if (arguments.Count == 1)
                        {
                            response = "Usage: audio play {path}";
                            return false;
                        }

                        string path = arguments.At(1);

                        if (!File.Exists(path))
                        {
                            response = $"No files exist inside that path.\nPath: {path}";
                            return false;
                        }

                        audioPlayer.Enqueue(path, -1);
                        audioPlayer.Volume = 100;
                        audioPlayer.Play(0);

                        response = "Playing...";
                        return true;
                    }
                case "s":
                case "stop":
                    if (!sender.CheckPermission("audioplayer.stop"))
                    {
                        response = "You dont have perms to do that";
                        return false;
                    }
                    audioPlayer.Stoptrack(true);
                    response = "Stop";
                    return true;
                case "v":
                case "vol":
                case "volume":
                    if (!sender.CheckPermission("audioplayer.volume"))
                    {
                        response = "You dont have perms to do that";
                        return false;
                    }
                    if (arguments.Count == 1)
                    {
                        response = "Usage: audio volume {Number}";
                        return false;
                    }
                    if (!float.TryParse(arguments.At(1), out float volume))
                    {
                        response = "Couldn't parse that volume, make sure it is a integer between 0 and 100";
                        return false;
                    }
                    audioPlayer.Volume = volume;
                    response = "Setted up!";
                    return true;
                case "add":
                case "spawn":
                case "create":
                    if (!sender.CheckPermission("audioplayer.createbot"))
                    {
                        response = "You dont have perms to do that";
                        return false;
                    }
                    if (!BotReady)
                    {
                        response = "Bot already exists!";
                        return false;
                    }
                    BotReady = true;
                    Plugin.SpawnDummy(player.Id, player.Position, player.Rotation, Plugin.plugin.Config.BotName);
                    response = "Bot was create server";
                    return true;
                case "kick":
                case "destroy":
                    if (!sender.CheckPermission("audioplayer.kickbot"))
                    {
                        response = "You dont have perms to do that";
                        return false;
                    }
                    if (!BotReady)
                    {
                        response = "Bot is already missing!";
                        return false;
                    }
                    BotReady = false;
                    NetworkServer.RemovePlayerForConnection(Plugin.plugin.fakeConnection, newPlayer);
                    response = "Bot was kick server";
                    return true;
                case "c":
                case "channel":
                    if (!sender.CheckPermission("audioplayer.channel"))
                    {
                        response = "You dont have perms to do that";
                        return false;
                    }
                    if (arguments.Count == 1)
                    {
                        response = "Usage: audio channel {VoiceChatChannel}";
                        return false;
                    }
                    switch (arguments.At(1))
                    {
                        case "Intercom":
                            audioPlayer.BroadcastChannel = VoiceChatChannel.Intercom;
                            break;
                        case "Mimicry":
                            audioPlayer.BroadcastChannel = VoiceChatChannel.Mimicry;
                            break;
                        case "None":
                            audioPlayer.BroadcastChannel = VoiceChatChannel.None;
                            break;
                        case "Proximity":
                            audioPlayer.BroadcastChannel = VoiceChatChannel.Proximity;
                            break;
                        case "Radio":
                            audioPlayer.BroadcastChannel = VoiceChatChannel.Radio;
                            break;
                        case "RoundSummary":
                            audioPlayer.BroadcastChannel = VoiceChatChannel.RoundSummary;
                            break;
                        case "Scp1576":
                            audioPlayer.BroadcastChannel = VoiceChatChannel.Scp1576;
                            break;
                        case "ScpChat":
                            audioPlayer.BroadcastChannel = VoiceChatChannel.ScpChat;
                            break;
                        case "Spectator":
                            audioPlayer.BroadcastChannel = VoiceChatChannel.Spectator;
                            break;
                    }
                    response = $"Bot is now broadcasting in the channel - {audioPlayer.BroadcastChannel}";
                    return true;
                case "setnick":
                case "nick":
                case "name":
                case "n":
                    if (arguments.Count == 1)
                    {
                        response = "Usage: audio nick {Name}";
                        return false;
                    }
                    hubPlayer.nicknameSync.SetNick(arguments.At(1));
                    hubPlayer.nicknameSync.name = arguments.At(1);
                    response = $"Bot nickname has been changed to - {arguments.At(1)}";
                    return true;
            }
            response = "Error";
            return false;
        }
    }
}
