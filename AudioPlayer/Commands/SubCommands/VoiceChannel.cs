using AudioPlayer.API;
using CommandSystem;
using Exiled.Permissions.Extensions;
using System;
using VoiceChat;

namespace AudioPlayer.Commands.SubCommands;

public class VoiceChannel : ICommand, IUsageProvider
{
    public string Command => "voicechannel";

    public string[] Aliases => ["voice", "channel", "chan", "audiochannel"];

    public string Description => "Changes AudioPlayer Bot VoiceChannel";

    public string[] Usage => ["Bot ID", "VoiceChatChannel"];

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!sender.CheckPermission($"audioplayer.{Command}"))
        {
            response = $"You dont have perms to do that. Not enough perms: audioplayer.{Command}";
            return false;
        }

        if (arguments.Count <= 1)
        {
            response = "Usage: audio channel {Bot ID} {VoiceChatChannel}";
            return false;
        }

        if (!int.TryParse(arguments.At(0), out int id))
        {
            response = "Specify a number, other characters are not accepted";
            return true;
        }

        if (!VoiceChatChannel.TryParse(arguments.At(1), out VoiceChatChannel voiceChatChannel))
        {
            response = $"I couldn't find a VoiceChatChannel with the name - {arguments.At(1)}";
            return false;
        }

        if (AudioController.TryGetAudioPlayerContainer(id) is not API.Container.AudioPlayerBot hub)
        {
            response = $"Bot with the ID {id} was not found.";
            return false;
        }

        hub.VoiceChatChannel = voiceChatChannel;

        response = $"AudioChannel changed for ID {id} to {voiceChatChannel}";
        return true;
    }
}
