using AudioPlayer.Other;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using System.Linq;

namespace AudioPlayer.Commands.SubCommands
{
    public class NickName : ICommand, IUsageProvider
    {
        public string Command => "nickname";

        public string[] Aliases { get; } = { "setnickname", "setnick", "nick", "name" };

        public string Description => "Sets name of AudioPlayer Bot";

        public string[] Usage { get; } = { "Bot ID", "Text" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission($"audioplayer.{Command}"))
            {
                response = $"You dont have perms to do that. Not enough perms: audioplayer.{Command}";
                return false;
            }
            if (arguments.Count <= 1)
            {
                response = "Usage: audio nick {Bot ID} {Text}";
                return false;
            }
            int id = int.Parse(arguments.At(0));
            if (Plugin.plugin.FakeConnectionsIds.TryGetValue(id, out FakeConnectionList hub))
            {
                string nickname = string.Join(" ", arguments.Where(x => arguments.At(0) != x));
                hub.hubPlayer.nicknameSync.SetNick(nickname);
                response = $"Set the nickname ID {id}, at {nickname}";
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
