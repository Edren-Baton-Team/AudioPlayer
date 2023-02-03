using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using Mirror;
using System;
using System.IO;
using System.Linq;
using VoiceChat;

namespace AudioPlayer.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Play : ICommand, IUsageProvider
    {
        public string Command { get; } = "audio";

        public string Description { get; } = "Audio Player command";

        public string[] Aliases { get; } = { "au" };

        public string[] Usage { get; set; } = new string[] { "play/stop/volume/kick/" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            var audioPlayer = Plugin.plugin.audioplayer;
            var hubPlayer = Plugin.plugin.hubPlayer;
            var fake = Plugin.plugin.fakeConnection;
            var BotReady = Plugin.plugin.BotReady;
            if (arguments.Count == 0)
            {
                response = "" +
                    "\nArgs: " +
                    "\naudio spawn - Spawn AudioPlayer Bot" +
                    "\naudio destroy - Destroys AudioPlayer Bot" +
                    "\naudio play {Path} - Playing Audio From Path" +
                    "\naudio volume {volume} - Set AudioPlayer Volume" +
                    "\naudio channel {VoiceChatChannel} - Changes bot VoiceChannel" +
                    "\naudio nickname {text} - Sets name of bot" +
                    "\naudio stop - Stop music";
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

                        string path = string.Join(" ", arguments.ToArray());

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
                    if (BotReady)
                    {
                        response = "Bot already exists!";
                        return false;
                    }
                    BotReady = true;
                    Plugin.SpawnDummy(133, Plugin.plugin.Config.BotName);
                    response = "Bot was kick server";
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
                    NetworkServer.RemovePlayerForConnection(Plugin.plugin.fakeConnection, Plugin.plugin.newPlayer);
                    response = "Bot was kick server";
                    return true;
                case "c":
                case "channel":
                    if (!sender.CheckPermission("audioplayer.channel"))
                    {
                        response = "You dont have perms to do that";
                        return false;
                    }
                    int id = int.Parse(arguments.At(1));
                    audioPlayer.BroadcastChannel = (VoiceChatChannel)Enum.Parse(typeof(VoiceChatChannel), arguments.At(2));
                    response = $"Bot is now broadcasting in the channel - {audioPlayer.BroadcastChannel}";
                    return true;
                case "setnick":
                case "nick":
                case "name":
                case "n":
                    hubPlayer.nicknameSync.SetNick(arguments.At(2));
                    response = $"Bot nickname has been changed to - {arguments.At(2)}";
                    return true;
            }
            response = "Done";
            return true;
        }
    }
}
