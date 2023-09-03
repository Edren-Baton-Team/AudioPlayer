using AudioPlayer.Other;
using CommandSystem;
using Exiled.Permissions.Extensions;
using System;

namespace AudioPlayer.Commands.SubCommands;

public class Enqueue : ICommand, IUsageProvider
{
    public string Command => "enqueue";

    public string[] Aliases { get; } = Array.Empty<string>();

    public string Description => "Adds audio to the queue";

    public string[] Usage { get; } = { "Bot ID", "Path", "Position" };

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
        int id = int.Parse(arguments.At(0));
        if (Extensions.TryGetAudioBot(id, out FakeConnectionList hub))
        {
            hub.audioplayer.Enqueue(arguments.At(1), arguments.Count >= 4 ? Convert.ToInt32(arguments.At(2)) : -1);
        }
        else
        {
            response = $"Bot with the ID {id} was not found.";
            return false;
        }
        response = $"Moved the audio playback at ID {id} to the position {(arguments.Count >= 3 ? Convert.ToInt32(arguments.At(2)) : -1)}, on the path {arguments.At(1)}";
        return true;
    }
}
