using AudioPlayer.API;
using AudioPlayer.API.Container;
using AudioPlayer.Other;
using CommandSystem;
using Exiled.Permissions.Extensions;
using System;

namespace AudioPlayer.Commands.SubCommands;

public class Add : ICommand, IUsageProvider
{
    public string Command => "add";

    public string[] Aliases => ["create", "cr", "fake", "bot"];

    public string Description => "Spawn AudioPlayer bot";

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
            response = "Usage: audio add {Bot ID}";
            return false;
        }

        if (!int.TryParse(arguments.At(0), out int id))
        {
            response = "Specify a number, other characters are not accepted";
            return true;
        }

        if (id.IsAudioPlayer())
        {
            response = $"Bot with an ID {id} already exists";
            return false;
        }

        AudioPlayerBot.SpawnDummy(id: id);

        response = $"Added bot with ID {id}";
        return true;
    }
}
