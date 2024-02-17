using AudioPlayer.Other;
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

            if (File.Exists(path))
            {
                Log.Debug("The full path was specified, I'm skipping it");
            }
            else if (File.Exists(Plugin.plugin.AudioPath + "/" + path))
            {
                path = Plugin.plugin.AudioPath + "/" + path;
                Log.Debug("An incomplete path was specified, I am looking for the .ogg file in the audio folder");
            }
            else
            {
                response = $"No files exist inside that path.\nPath: {path}";
                return false;
            }

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