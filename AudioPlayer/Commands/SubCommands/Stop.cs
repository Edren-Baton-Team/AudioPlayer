using AudioPlayer.API;
using CommandSystem;
using Exiled.Permissions.Extensions;
using System;

namespace AudioPlayer.Commands.SubCommands;

public class Stop : ICommand, IUsageProvider
{
    public string Command => "stop";
    public string[] Aliases => [];
    public string Description => "Stop AudioPlayer bot audio playback";
    public string[] Usage => ["Bot ID"];

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!sender.CheckPermission($"audioplayer.{Command}"))
        {
            response = $"You dont have perms to do that. Not enough perms: audioplayer.{Command}";
            return false;
        }

        if (arguments.Count == 0)
        {
            response = "Usage: audio stop {Bot ID}";
            return false;
        }

        if (!int.TryParse(arguments.At(0), out int id))
        {
            response = "Specify a number, other characters are not accepted";
            return true;
        }

        if (AudioController.TryGetAudioPlayerContainer(id) is not API.Container.AudioPlayerBot hub)
        {
            response = $"Bot with the ID {id} was not found.";
            return false;
        }

        hub.AudioPlayerBase.Stoptrack(true);

        response = $"Stopped audio playback at an ID {id}";
        return true;
    }
}
