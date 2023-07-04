using AudioPlayer.Other;
using CommandSystem;
using Exiled.Permissions.Extensions;
using System;
using System.IO;
using System.Linq;
using static AudioPlayer.Plugin;

namespace AudioPlayer.Commands.SubCommands
{
    public class Play : ICommand, IUsageProvider
    {
        public string Command => "play";

        public string[] Aliases { get; } = { "playback", "replay" };

        public string Description => "Play the sound by path";

        public string[] Usage { get; } = { "Bot ID", "Path" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("audioplayer.play"))
            {
                response = "You dont have perms to do that. Not enough perms: audioplayer.play";
                return false;
            }
            if (arguments.Count <= 1)
            {
                response = "Usage: audio play {Bot ID} {Path}";
                return false;
            }
            int id = int.Parse(arguments.At(0));
            if (plugin.FakeConnectionsIds.TryGetValue(id, out FakeConnectionList hub))
            {
                string path = string.Join(" ", arguments.Where(x => arguments.At(0) != x));

                if (!File.Exists(path))
                {
                    response = $"No files exist inside that path.\nPath: {path}";
                    return false;
                }

                hub.audioplayer.Enqueue(path, -1);
                hub.audioplayer.Play(0);
                response = $"Started playing audio at ID {id}, by path {path}";
                return true;
            }
            else
            {
                response = "No ID found";
                return false;
            }
        }
    }
}
