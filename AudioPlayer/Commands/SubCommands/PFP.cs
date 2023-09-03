using AudioPlayer.API;
using AudioPlayer.Other;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AudioPlayer.Commands.SubCommands;

public class PFP : ICommand, IUsageProvider
{
    public string Command => "playfromplayers";

    public string[] Aliases { get; } = { "pfp" };

    public string Description => "AudioPlayer Bot plays the sound in a certain player";

    public string[] Usage { get; } = { "Bot ID", "PlayerList", "Path" };

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
        int id = int.Parse(arguments.At(0));
        if (Extensions.TryGetAudioBot(id, out FakeConnectionList hub))
        {
            string path = string.Join(" ", arguments.Where(x => arguments.At(0) != x && arguments.At(1) != x));
            string textToResponse = string.Empty;
            if (!File.Exists(path))
            {
                response = $"No files exist inside that path.\nPath: {path}";
                return false;
            }
            List<Player> list = new List<Player>();

            foreach (string s in arguments.At(1).Trim('.').Split('.'))
            {
                var pl = Player.Get(s);
                textToResponse += pl.Nickname + ", ";
                list.Add(pl);
            }

            if (list == null || list.Count == 0)
            {
                response = "No players found";
                return false;
            }

            hub.audioplayer.Enqueue(path, -1);
            foreach (Player ply in list)
            {
                AudioController.PlayFromFilePlayer(new List<int>() { ply.Id }, path, id: id);
            }
            response = $"Started playing audio from ID {id}, players {textToResponse}, along path {path}";
            return true;
        }
        else
        {
            response = $"Bot with the ID {id} was not found.";
            return false;
        }
    }
}
