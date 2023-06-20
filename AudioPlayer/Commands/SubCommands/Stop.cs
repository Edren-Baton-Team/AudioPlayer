using AudioPlayer.Other;
using CommandSystem;
using Exiled.Permissions.Extensions;
using System;
using static AudioPlayer.Plugin;

namespace AudioPlayer.Commands.SubCommands
{
    public class Stop : ICommand, IUsageProvider
    {
        public string Command => "stop";

        public string[] Aliases { get; } = Array.Empty<string>();

        public string Description => "Stop AudioPlayer bot audio playback";

        public string[] Usage { get; } = { "Bot ID" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("audioplayer.stop"))
            {
                response = "You dont have perms to do that. Not enough perms: audioplayer.stop";
                return false;
            }
            if (arguments.Count == 0)
            {
                response = "Usage: audio stop {Bot ID}";
                return false;
            }
            int id = int.Parse(arguments.At(0));
            if (plugin.FakeConnectionsIds.TryGetValue(id, out FakeConnectionList hub))
                hub.audioplayer.Stoptrack(true);
            if (plugin.LobbySong)
                plugin.LobbySong = false;
            response = $"Stopped audio playback at an ID {id}";
            return true;
        }
    }
}
