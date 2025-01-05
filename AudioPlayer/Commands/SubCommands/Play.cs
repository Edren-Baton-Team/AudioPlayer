using AudioPlayer.API;
using AudioPlayer.Other;
using CommandSystem;
using Exiled.Permissions.Extensions;
using System;
using System.Linq;

namespace AudioPlayer.Commands.SubCommands;

public class Play : ICommand, IUsageProvider
{
    public string Command => "play";

    public string[] Aliases => ["playback", "replay"];

    public string Description => "Play the sound by path";

    public string[] Usage => ["Bot ID", "Path"];

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

        if (AudioController.TryGetAudioPlayerContainer(id) is not API.Container.AudioPlayerBot hub)
        {
            response = $"Bot with the ID {id} was not found.";
            return false;
        }

        string path = Extensions.PathCheck(string.Join(" ", arguments.Where(x => arguments.At(0) != x)));
        
        hub.AudioPlayerBase.Enqueue(path, -1);
        hub.AudioPlayerBase.Play(0);
        
        response = $"Started playing audio at ID {id}, by path {path}";
        return true;
    }
}