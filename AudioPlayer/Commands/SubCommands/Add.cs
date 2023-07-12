using AudioPlayer.Other;
using CommandSystem;
using Exiled.Permissions.Extensions;
using System;

namespace AudioPlayer.Commands.SubCommands
{
    public class Add : ICommand, IUsageProvider
    {
        public string Command => "add";

        public string[] Aliases { get; } = { "create", "cr", "fake", "bot" };

        public string Description => "Spawn AudioPlayer bot";

        public string[] Usage { get; } = { "Bot ID" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("audioplayer.add"))
            {
                response = "You dont have perms to do that. Not enough perms: audioplayer.add";
                return false;
            }
            if (arguments.Count == 0)
            {
                response = "Usage: audio add {Bot ID}";
                return false;
            }
            int id = int.Parse(arguments.At(0));
            Plugin.plugin.handlers.SpawnDummy(id: id);
            response = $"Added bot with ID {id}";
            return true;
        }
    }
}
