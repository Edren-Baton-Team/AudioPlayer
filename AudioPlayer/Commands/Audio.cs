using CommandSystem;
using Exiled.Permissions.Extensions;
using RemoteAdmin;
using SCPSLAudioApi.AudioCore;
using System;
using System.IO;
using VoiceChat;

namespace AudioPlayer;
[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class AudioCommand : ICommand
{
    public string Command { get; } = "audio";

    public string[] Aliases { get; } = { "au" };

    public string Description { get; } = "Audio Command";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!(sender is PlayerCommandSender raSender))
        {
            response = "Console cant execute this command!";
            return false;
        }

        if (!sender.CheckPermission("audioplayer.audio"))
        {
            response = "You dont have permission to run this command.";
            return false;
        }

        AudioPlayerBase player = AudioPlayerBase.Get(raSender.ReferenceHub);

        if (arguments.Count == 0)
        {
            response = "" +
                "\nArgs:" +
                "\naudio play {index} - starts playing at the given index" +
                "\naudio stop - Stop music" +
                "\naudio volume {volume} - sets the volume" +
                "\naudio channel {VoiceChatChannel} - Changes bot VoiceChannel" +
                "\naudio loop false/true - Make it cyclic playback?" +
                "\naudio enqueue {path} {position} - Adds audio to the queue" +
                "\naudio nick {Name} - Sets name of bot";
            return false;
        }

        switch (arguments.At(0).ToLower())
        {
            case "setnick":
            case "nick":
            case "name":
            case "n":
                {
                    if (!sender.CheckPermission("audioplayer.nick"))
                    {
                        response = "You dont have perms to do that. Not enough perms: audioplayer.nick";
                        return false;
                    }
                    if (arguments.Count == 1)
                    {
                        response = "Usage: audio nick {Name}";
                        return false;
                    }
                    Plugin.plugin.FakeConnectionsIds[99].hubPlayer.nicknameSync.SetNick(arguments.At(1));
                }
                break;
            case "p":
            case "play":
                {
                    if (!sender.CheckPermission("audioplayer.play"))
                    {
                        response = "You dont have perms to do that. Not enough perms: audioplayer.play";
                        return false;
                    }
                    if (arguments.Count == 1)
                    {
                        response = "Usage: audio play {path}";
                        return false;
                    }
                    string path = arguments.At(1);
                    if (!File.Exists(path))
                    {
                        response = $"No files exist inside that path.\nPath: {path}";
                        return false;
                    }
                    Plugin.plugin.FakeConnectionsIds[99].audioplayer.Enqueue(path, -1);
                    Plugin.plugin.FakeConnectionsIds[99].audioplayer.Play(0);
                }
                break;
            case "enqueue":
                {
                    if (!sender.CheckPermission("audioplayer.enqueue"))
                    {
                        response = "You dont have perms to do that. Not enough perms: audioplayer.enqueue";
                        return false;
                    }
                    Plugin.plugin.FakeConnectionsIds[99].audioplayer.Enqueue(arguments.At(1), arguments.Count >= 4 ? Convert.ToInt32(arguments.At(2)) : -1);
                }
                break;
            case "s":
            case "stop":
                {
                    if (!sender.CheckPermission("audioplayer.stop"))
                    {
                        response = "You dont have perms to do that. Not enough perms: audioplayer.stop";
                        return false;
                    }
                    if (Plugin.plugin.LobbySong)
                        Plugin.plugin.LobbySong = false;
                    Plugin.plugin.FakeConnectionsIds[99].audioplayer.Stoptrack(true);
                }
                break;
            case "v":
            case "vol":
            case "volume":
                {
                    if (!sender.CheckPermission("audioplayer.volume"))
                    {
                        response = "You dont have perms to do that. Not enough perms: audioplayer.volume";
                        return false;
                    }
                    if (arguments.Count == 1)
                    {
                        response = "Usage: audio volume {Number}";
                        return false;
                    }
                    Plugin.plugin.FakeConnectionsIds[99].audioplayer.Volume = Convert.ToInt32(arguments.At(1));
                }
                break;
            case "chan":
            case "channel":
            case "audiochannel":
                {
                    if (!sender.CheckPermission("audioplayer.channel"))
                    {
                        response = "You dont have perms to do that. Not enough perms: audioplayer.channel";
                        return false;
                    }
                    if (arguments.Count == 1)
                    {
                        response = "Usage: audio channel {VoiceChatChannel}";
                        return false;
                    }
                    Plugin.plugin.FakeConnectionsIds[99].audioplayer.BroadcastChannel = (VoiceChatChannel)Enum.Parse(typeof(VoiceChatChannel), arguments.At(1));
                }
                break;
            case "loop":
                {
                    if (!sender.CheckPermission("audioplayer.loop"))
                    {
                        response = "You dont have perms to do that. Not enough perms: audioplayer.loop";
                        return false;
                    }
                    if (arguments.Count == 1)
                    {
                        response = "Usage: audio loop false/true";
                        return false;
                    }
                    Plugin.plugin.FakeConnectionsIds[99].audioplayer.Loop = Convert.ToBoolean(arguments.At(1));
                }
                break;
        }
        response = "Done";
        return true;
    }
}