using AudioPlayer.API;
using AudioPlayer.Other;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using System.Linq;

namespace AudioPlayer.Commands.SubCommands;

public class NickName : ICommand, IUsageProvider
{
    public string Command => "nickname";

    public string[] Aliases => ["setnickname", "setnick", "nick", "name"];

    public string Description => "Sets name of AudioPlayer Bot";

    public string[] Usage => ["Bot ID", "Text"];

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
        if (!int.TryParse(arguments.At(0), out int id))
        {
            response = "Specify a number, other characters are not accepted";
            return true;
        }
        if (AudioController.TryGetAudioPlayerContainer(id) is API.Container.AudioPlayerBot hub)
        {
            string nickname = string.Join(" ", arguments.Where(x => arguments.At(0) != x));
            hub.Player.ReferenceHub.nicknameSync.Network_myNickSync = nickname;
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
