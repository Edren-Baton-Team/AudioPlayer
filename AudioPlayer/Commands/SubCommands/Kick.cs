using AudioPlayer.API;
using AudioPlayer.Other;
using CommandSystem;
using Exiled.Permissions.Extensions;
using System;

namespace AudioPlayer.Commands.SubCommands
{
    public class Kick : ICommand, IUsageProvider
    {
        public string Command => "kick";

        public string[] Aliases { get; } = { "delete", "del", "remove", "rem", "destroy" };

        public string Description => "Kick AudioPlayer Bot";

        public string[] Usage { get; } = { "Bot ID" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission($"audioplayer.{Command}"))
            {
                response = $"You dont have perms to do that. Not enough perms: audioplayer.{Command}";
                return false;
            }
            if (arguments.Count == 0)
            {
                response = "Usage: audio kick {Bot ID}";
                return false;
            }
            int id = int.Parse(arguments.At(0));
            if (Plugin.plugin.FakeConnectionsIds.TryGetValue(id, out FakeConnectionList hub))
            {
                AudioController.DisconnectDummy(id);
            }
            else
            {
                response = $"Bot with the ID {id} was not found.";
                return false;
            }
            response = $"Kicked the bot out of the ID {id}";
            return true;
        }
    }
}
