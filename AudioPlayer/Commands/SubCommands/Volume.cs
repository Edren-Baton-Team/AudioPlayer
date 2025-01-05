using AudioPlayer.API;
using CommandSystem;
using Exiled.Permissions.Extensions;
using System;

namespace AudioPlayer.Commands.SubCommands;

public class Volume : ICommand, IUsageProvider
{
    public string Command => "volume";
    public string[] Aliases => ["vol", "v"];
    public string Description => "Sets the volume of the AudioPlayer Bot";
    public string[] Usage => ["Bot ID", "Number"];

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

        if (!int.TryParse(arguments.At(0), out int id))
        {
            response = "Specify a number, other characters are not accepted";
            return true;
        }

        if (!float.TryParse(arguments.At(1), out float volume))
        {
            response = "Couldn't parse that volume, make sure it is a integer between 0 and 100";
            return false;
        }

        if (AudioController.TryGetAudioPlayerContainer(id) is not API.Container.AudioPlayerBot hub)
        {
            response = $"Bot with the ID {id} was not found.";
            return false;
        }

        hub.AudioPlayerBase.Volume = volume;

        response = $"The volume has been changed for ID {id} to {volume}";
        return true;
    }
}
