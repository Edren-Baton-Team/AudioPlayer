﻿using AudioPlayer.Other;
using CommandSystem;
using Exiled.Permissions.Extensions;
using System;
using static AudioPlayer.Plugin;

namespace AudioPlayer.Commands.SubCommands;

public class Stop : ICommand, IUsageProvider
{
    public string Command => "stop";

    public string[] Aliases => Array.Empty<string>();

    public string Description => "Stop AudioPlayer bot audio playback";

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
            response = "Usage: audio stop {Bot ID}";
            return false;
        }
        if (!int.TryParse(arguments.At(0), out int id))
        {
            response = "Specify a number, other characters are not accepted";
            return true;
        }
        if (Extensions.TryGetAudioBot(id, out FakeConnectionList hub))
        {
            hub.audioplayer.Stoptrack(true);
            if (plugin.LobbySong != null)
                plugin.LobbySong = null;
        }
        else
        {
            response = $"Bot with the ID {id} was not found.";
            return false;
        }
        response = $"Stopped audio playback at an ID {id}";
        return true;
    }
}
