using AudioPlayer.Other;
using CommandSystem;
using Exiled.Permissions.Extensions;
using System;
using VoiceChat;

namespace AudioPlayer.Commands.SubCommands;

public class VoiceChannel : ICommand, IUsageProvider
{
    public string Command => "voicechannel";

    public string[] Aliases { get; } = { "voice", "channel", "chan", "audiochannel" };

    public string Description => "Changes AudioPlayer Bot VoiceChannel";

    public string[] Usage { get; } = { "Bot ID", "VoiceChatChannel" };

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
        int id = int.Parse(arguments.At(0));
        if (Extensions.TryGetAudioBot(id, out FakeConnectionList hub))
        {
            hub.audioplayer.BroadcastChannel = (VoiceChatChannel)Enum.Parse(typeof(VoiceChatChannel), arguments.At(1));
            response = $"AudioChannel changed for ID {id} to {(VoiceChatChannel)Enum.Parse(typeof(VoiceChatChannel), arguments.At(1))}";
            return true;
        }
        else
        {
            response = $"Bot with the ID {id} was not found.";
            return false;
        }
    }
}
