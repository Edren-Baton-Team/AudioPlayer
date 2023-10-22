using AudioPlayer.Other;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using System.Collections.Generic;

namespace AudioPlayer.Commands.SubCommands;

public class SPFP : ICommand, IUsageProvider
{
    public string Command => "stopplayfromplayers";

    public string[] Aliases { get; } = { "spfp", "stoppfp" };

    public string Description => "AudioPlayer Bot stop audio playback for certain players";

    public string[] Usage { get; } = { "Bot ID", "PlayerList" };

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!sender.CheckPermission($"audioplayer.{Command}"))
        {
            response = $"You dont have perms to do that. Not enough perms: audioplayer.{Command}";
            return false;
        }
        if (arguments.Count <= 1)
        {
            response = "Usage: audio spfp {Bot ID} {PlayerList}";
            return false;
        }
        if (!int.TryParse(arguments.At(0), out int id))
        {
            response = "Specify a number, other characters are not accepted";
            return true;
        }

        List<int> list = new();
        string textToResponse = string.Empty;

        foreach (string s in arguments.At(1).Trim('.').Split('.'))
        {
            Player players = Player.Get(s);
            textToResponse += players.Nickname + ", ";
            list.Add(players.Id);
        }
        if (list == null || list.Count == 0)
        {
            response = "No players found";
            return false;
        }

        if (Extensions.TryGetAudioBot(id, out FakeConnectionList hub))
        {
            foreach (var playersList in list)
            {
                hub.audioplayer.BroadcastTo.Remove(playersList);
            }
            response = $"Stopped the sound at ID {id} for the next player: {textToResponse}";
            return true;
        }
        else
        {
            response = $"Bot with the ID {id} was not found.";
            return false;
        }
    }
}
