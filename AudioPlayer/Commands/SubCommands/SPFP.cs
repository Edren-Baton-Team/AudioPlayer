using AudioPlayer.API;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AudioPlayer.Commands.SubCommands;

public class SPFP : ICommand, IUsageProvider
{
    public string Command => "stopplayfromplayers";

    public string[] Aliases => ["spfp", "stoppfp"];

    public string Description => "AudioPlayer Bot stop audio playback for certain players";

    public string[] Usage => ["Bot ID", "PlayerList"];

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

        string[] playerNames = arguments.At(1).Trim('.').Split('.');
        Player[] list = playerNames.Select(name => Player.Get(name)).ToArray();

        if (!list.Any())
        {
            response = "No players found";
            return false;
        }

        if (AudioController.TryGetAudioPlayerContainer(id) is not API.Container.AudioPlayerBot hub)
        {
            response = $"Bot with the ID {id} was not found.";
            return false;
        }

        foreach (var target in list)
        {
            hub.AudioPlayerBase.BroadcastTo.Remove(target.Id);
        }

        response = $"Stopped the sound at ID {id} for the next player: {string.Join(", ", list.Select(player => player.Id))}";
        return true;
    }
}
