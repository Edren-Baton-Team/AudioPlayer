using AudioPlayer.API;
using CommandSystem;
using Exiled.Permissions.Extensions;
using System;
using static System.Array;

namespace AudioPlayer.Commands.SubCommands;

public class Enqueue : ICommand, IUsageProvider
{
    public string Command => "enqueue";

    public string[] Aliases => [];

    public string Description => "Adds audio to the queue";

    public string[] Usage => ["Bot ID", "Path", "Position"];

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!sender.CheckPermission($"audioplayer.{Command}"))
        {
            response = $"You dont have perms to do that. Not enough perms: audioplayer.{Command}";
            return false;
        }

        if (arguments.Count <= 2)
        {
            response = "Usage: audio enqueue {Bot ID} {Path} {Position}";
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

        hub.AudioPlayerBase.Enqueue(arguments.At(1), arguments.Count >= 4 ? Convert.ToInt32(arguments.At(2)) : -1);

        response = $"Moved the audio playback at ID {id} to the position {(arguments.Count >= 3 ? Convert.ToInt32(arguments.At(2)) : -1)}, on the path {arguments.At(1)}";
        return true;
    }
}