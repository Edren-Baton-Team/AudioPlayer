﻿using AudioPlayer.Other;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using System.IO;
using System.Linq;

namespace AudioPlayer.Commands.SubCommands;

public class Play : ICommand, IUsageProvider
{
    public string Command => "play";

    public string[] Aliases { get; } = { "playback", "replay" };

    public string Description => "Play the sound by path";

    public string[] Usage { get; } = { "Bot ID", "Path" };

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!sender.CheckPermission($"audioplayer.{Command}"))
        {
            response = $"You dont have perms to do that. Not enough perms: audioplayer.{Command}";
            return false;
        }
        if (arguments.Count <= 1)
        {
            response = "Usage: audio play {Bot ID} {Path}";
            return false;
        }
        if (!int.TryParse(arguments.At(0), out int id))
        {
            response = "Specify a number, other characters are not accepted";
            return true;
        }
        if (Extensions.TryGetAudioBot(id, out FakeConnectionList hub))
        {
            string path = string.Join(" ", arguments.Where(x => arguments.At(0) != x));
            path = Extensions.PathCheck(path);

            hub.audioplayer.Enqueue(path, -1);
            hub.audioplayer.Play(0);
            response = $"Started playing audio at ID {id}, by path {path}";
            return true;
        }
        else
        {
            response = $"Bot with the ID {id} was not found.";
            return false;
        }
    }
}