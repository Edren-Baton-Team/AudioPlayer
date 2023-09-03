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
            if (!sender.CheckPermission($"audioplayer.{Command}"))
            {
                response = $"You dont have perms to do that. Not enough perms: audioplayer.{Command}";
                return false;
            }
            if (arguments.Count == 0)
            {
                response = "Usage: audio add {Bot ID}";
                return false;
            }
            int id = int.Parse(arguments.At(0));

            if (Extensions.IsThereAudioBot(id))
            {
                response = $"Bot with an ID {id} already exists";
                return false;
            }

            if (!Extensions.TryGetAudioBot(id, out FakeConnectionList hub))
            {
                Extensions.SpawnDummy(id: id);
            }
            else
            {
                response = $"Bot with the ID {id} was not found.";
                return false;
            }
            response = $"Added bot with ID {id}";
            return true;
        }
    }
}
