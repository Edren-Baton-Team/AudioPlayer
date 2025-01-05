using AudioPlayer.API;
using AudioPlayer.Other;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AudioPlayer.Commands.SubCommands;

public class PFP : ICommand, IUsageProvider
{
    public string Command => "playfromplayers";
    public string[] Aliases => ["pfp"];
    public string Description => "AudioPlayer Bot plays the sound in a certain player";
    public string[] Usage => ["Bot ID", "PlayerList", "Path"];

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!sender.CheckPermission($"audioplayer.{Command}"))
        {
            response = $"You dont have perms to do that. Not enough perms: audioplayer.{Command}";
            return false;
        }

        if (arguments.Count <= 2)
        {
            response = "Usage: audio pfp {Bot ID} {PlayerList} {Path}";
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

        string path = string.Join(" ", arguments.Where(x => arguments.At(0) != x && arguments.At(1) != x));

        string[] playerNames = arguments.At(1).Trim('.').Split('.');
        Player[] list = playerNames.Select(name => Player.Get(name)).ToArray();

        if (!list.Any())
        {
            response = "No players found";
            return false;
        }

        hub.AudioPlayerBase.Enqueue(Extensions.PathCheck(path), -1);
        hub.PlayFromFilePlayer(list.Select(p => p.Id).ToList(), path);

        response = $"Started playing audio from ID {id}, players {string.Join(", ", list.Select(player => player.Id))}, along path {path}";
        return true;
    }
}
