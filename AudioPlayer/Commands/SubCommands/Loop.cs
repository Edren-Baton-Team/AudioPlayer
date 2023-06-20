using AudioPlayer.Other;
using CommandSystem;
using Exiled.Permissions.Extensions;
using System;

namespace AudioPlayer.Commands.SubCommands
{
    public class Loop : ICommand, IUsageProvider
    {
        public string Command => "loop";

        public string[] Aliases { get; } = Array.Empty<string>();

        public string Description => "Make the AudioPlayer Bot loop playback";

        public string[] Usage { get; } = { "Bot ID", "false/true" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("audioplayer.loop"))
            {
                response = "You dont have perms to do that. Not enough perms: audioplayer.loop";
                return false;
            }
            if (arguments.Count <= 1)
            {
                response = "Usage: audio loop {Bot ID} {false/true}";
                return false;
            }
            int id = int.Parse(arguments.At(0));
            if (Plugin.plugin.FakeConnectionsIds.TryGetValue(id, out FakeConnectionList hub))
                hub.audioplayer.Loop = Convert.ToBoolean(arguments.At(1));
            response = $"Looping is enabled for ID {id}";
            return true;
        }
    }
}
