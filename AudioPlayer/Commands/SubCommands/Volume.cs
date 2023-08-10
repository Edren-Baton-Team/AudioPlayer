using AudioPlayer.Other;
using CommandSystem;
using Exiled.Permissions.Extensions;
using System;

namespace AudioPlayer.Commands.SubCommands
{
    public class Volume : ICommand, IUsageProvider
    {
        public string Command => "volume";

        public string[] Aliases { get; } = { "vol", "v" };

        public string Description => "Sets the volume of the AudioPlayer Bot";

        public string[] Usage { get; } = { "Bot ID", "Number" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission($"audioplayer.{Command}"))
            {
                response = $"You dont have perms to do that. Not enough perms: audioplayer.{Command}";
                return false;
            }
            if (arguments.Count <= 1)
            {
                response = "Usage: audio volume {Bot ID} {Number}";
                return false;
            }
            int id = int.Parse(arguments.At(0));
            if (Plugin.plugin.FakeConnectionsIds.TryGetValue(id, out FakeConnectionList hub))
            {
                hub.audioplayer.Volume = float.Parse(arguments.At(1));
                response = $"The volume has been changed for ID {id} to {float.Parse(arguments.At(1))}";
                return true;
            }
            else
            {
                response = $"Bot with the ID {id} was not found.";
                return false;
            }
        }
    }
}
