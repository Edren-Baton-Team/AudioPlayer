using AudioPlayer.Other;
using CommandSystem;
using Exiled.Permissions.Extensions;
using System;

namespace AudioPlayer.Commands.SubCommands;

public class Loop : ICommand, IUsageProvider
{
    public string Command => "loop";

    public string[] Aliases { get; } = Array.Empty<string>();

    public string Description => "Make the AudioPlayer Bot loop playback";

    public string[] Usage { get; } = { "Bot ID", "false/true" };

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!sender.CheckPermission($"audioplayer.{Command}"))
        {
            response = "You dont have perms to do that. Not enough perms: audioplayer.{Command}";
            return false;
        }
        if (arguments.Count <= 1)
        {
            response = "Usage: audio loop {Bot ID} {false/true}";
            return false;
        }
        if (!int.TryParse(arguments.At(0), out int id))
        {
            response = "Specify a number, other characters are not accepted";
            return true;
        }
        if (Extensions.TryGetAudioBot(id, out FakeConnectionList hub))
        {
            hub.audioplayer.Loop = Convert.ToBoolean(arguments.At(1));
            response = $"Looping is enabled for ID {id}";
            return true;
        }
        else
        {
            response = $"Bot with the ID {id} was not found.";
            return false;
        }

    }
}
