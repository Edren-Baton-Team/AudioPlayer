using AudioPlayer.API;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using RemoteAdmin;
using SCPSLAudioApi.AudioCore;
using System;
using System.Collections.Generic;
using System.IO;
using VoiceChat;
using static AudioPlayer.Plugin;

namespace AudioPlayer.Commands;
[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class AudioCommand : ICommand
{
    public string Command { get; } = "audiocommand";

    public string[] Aliases { get; } = Plugin.plugin.Config.CommandName;

    public string Description { get; } = "Audio Command";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!(sender is PlayerCommandSender raSender))
        {
            response = "Console cant execute this command!";
            return false;
        }
        Player user = Player.Get(sender);
        if (!sender.CheckPermission("audioplayer.audio"))
        {
            response = "You dont have permission to run this command.";
            return false;
        }

        AudioPlayerBase player = AudioPlayerBase.Get(raSender.ReferenceHub);

        if (arguments.Count == 0)
        {
            var v = plugin.Config.CommandName;

            response = "" +
                "\nArgs:" +
                "\naudio add {Bot ID} - spawns a dummyplayer" +
                "\naudio kick {Bot ID} - destroys a dummyplayer" +
                "\naudio play {Bot ID} {index} - Starts playing at the given index" +
                "\naudio pfp {Bot ID} {Players} {index} - Plays music to a specific player" +
                "\naudio stop {Bot ID} - Stop music" +
                "\naudio spfp {Bot ID} {Player List} - Stop a player from music" +
                "\naudio volume {Bot ID} {volume} - Sets the volume" +
                "\naudio channel {Bot ID} {VoiceChatChannel} - Changes bot VoiceChannel" +
                "\naudio loop {Bot ID} {false/true} - Make it cyclic playback?" +
                "\naudio enqueue {Bot ID} {path} {position} - Adds audio to the queue" +
                "\naudio nick {Bot ID} {Name} - Sets name of bot";
            return false;
        }
        switch (arguments.At(0).ToLower())
        {
            case "add":
            case "create":
            case "fake":
            case "bot":
                {
                    if (!sender.CheckPermission("audioplayer.add"))
                    {
                        response = "You dont have perms to do that. Not enough perms: audioplayer.add";
                        return false;
                    }
                    if (arguments.Count == 1)
                    {
                        response = "Usage: audio add {ID}";
                        return false;
                    }
                    plugin.handlers.SpawnDummy(id: int.Parse(arguments.At(1)));
                }
                break;
            case "kick":
            case "delete":
            case "remove":
            case "rem":
                {
                    if (!sender.CheckPermission("audioplayer.kick"))
                    {
                        response = "You dont have perms to do that. Not enough perms: audioplayer.kick";
                        return false;
                    }
                    if (arguments.Count == 1)
                    {
                        response = "Usage: audio kick {ID}";
                        return false;
                    }
                    AudioController.DisconnectDummy(int.Parse(arguments.At(1)));
                }
                break;
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
                    if (arguments.Count == 1 || arguments.Count == 2)
                    {
                        response = "Usage: audio nick {ID} {Name}";
                        return false;
                    }
                    int id = int.Parse(arguments.At(1));
                    if (plugin.FakeConnectionsIds.TryGetValue(id, out FakeConnectionList hub))
                    {
                        hub.hubPlayer.nicknameSync.SetNick(arguments.At(2));
                    }
                }
                break;
            case "pfp":
            case "playfromplayers":
                {
                    if (!sender.CheckPermission("audioplayer.playfromplayer"))
                    {
                        response = "You dont have perms to do that. Not enough perms: audioplayer.playfromplayer";
                        return false;
                    }
                    if (arguments.Count == 1 || arguments.Count == 2 || arguments.Count == 3)
                    {
                        response = "Usage: audio pfp {Bot ID} {Players} {index}";
                        return false;
                    }
                    int id = int.Parse(arguments.At(1));
                    if (plugin.FakeConnectionsIds.TryGetValue(id, out FakeConnectionList hub))
                    {
                        string path = arguments.At(3);
                        if (!File.Exists(path))
                        {
                            response = $"No files exist inside that path.\nPath: {path}";
                            return false;
                        }
                        List<Player> list = new List<Player>();
                        foreach (string s in arguments.At(2).Trim('.').Split('.'))
                            list.Add(Player.Get(s));

                        hub.audioplayer.Enqueue(path, -1);
                        foreach(Player ply in list)
                        {
                            API.AudioController.PlayFromFilePlayer(new List<int>() { ply.Id }, path, id: id); 
                        }
                    }
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
                    if (arguments.Count == 1 || arguments.Count == 2)
                    {
                        response = "Usage: audio play {ID} {path}";
                        return false;
                    }
                    int id = int.Parse(arguments.At(1));
                    if (plugin.FakeConnectionsIds.TryGetValue(id, out FakeConnectionList hub))
                    {
                        string path = arguments.At(2);

                        if (!File.Exists(path))
                        {
                            response = $"No files exist inside that path.\nPath: {path}";
                            return false;
                        }

                        hub.audioplayer.Enqueue(path, -1);
                        hub.audioplayer.Play(0);
                    }
                }
                break;
            case "enqueue":
                {
                    if (!sender.CheckPermission("audioplayer.enqueue"))
                    {
                        response = "You dont have perms to do that. Not enough perms: audioplayer.enqueue";
                        return false;
                    }
                    if (arguments.Count == 1 || arguments.Count == 2 || arguments.Count == 3)
                    {
                        response = "Usage: audio enqueue {Bot ID} {path} {position}";
                        return false;
                    }
                    int id = int.Parse(arguments.At(1));
                    if (Plugin.plugin.FakeConnectionsIds.TryGetValue(id, out FakeConnectionList hub))
                    {
                        hub.audioplayer.Enqueue(arguments.At(2), arguments.Count >= 4 ? Convert.ToInt32(arguments.At(3)) : -1);
                    }
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
                    if (arguments.Count == 1)
                    {
                        response = "Usage: audio stop {Bot ID}";
                        return false;
                    }
                    int id = int.Parse(arguments.At(1));
                    if (plugin.FakeConnectionsIds.TryGetValue(id, out FakeConnectionList hub))
                        hub.audioplayer.Stoptrack(true);
                    if (plugin.LobbySong)
                        plugin.LobbySong = false;
                }
                break;
            case "spfp":
            case "stoppfp":
                {
                    if (!sender.CheckPermission("audioplayer.stoppfp"))
                    {
                        response = "You dont have perms to do that. Not enough perms: audioplayer.stoppfp";
                        return false;
                    }
                    if (arguments.Count == 1)
                    {
                        response = "Usage: audio spfp {Bot ID} {Player List}";
                        return false;
                    }
                    int id = int.Parse(arguments.At(1));
                    List<int> list = new List<int>();
                    foreach (string s in arguments.At(2).Trim('.').Split('.'))
                    {
                        var pl = Player.Get(s);
                        list.Add(pl.Id);
                    }
                    AudioController.StopPlayerFromPlaying(list, id);
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
                    if (arguments.Count == 1 || arguments.Count == 2)
                    {
                        response = "Usage: audio volume {Bot ID} {volume}";
                        return false;
                    }
                    int id = int.Parse(arguments.At(1));
                    if (plugin.FakeConnectionsIds.TryGetValue(id, out FakeConnectionList hub))
                        hub.audioplayer.Volume = Convert.ToInt32(arguments.At(2));
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
                    if (arguments.Count == 1 || arguments.Count == 2)
                    {
                        response = "Usage: audio channel {Bot ID} {VoiceChatChannel}";
                        return false;
                    }
                    int id = int.Parse(arguments.At(1));
                    if (plugin.FakeConnectionsIds.TryGetValue(id, out FakeConnectionList hub))
                        hub.audioplayer.BroadcastChannel = (VoiceChatChannel)Enum.Parse(typeof(VoiceChatChannel), arguments.At(2));
                }
                break;
            case "loop":
                {
                    if (!sender.CheckPermission("audioplayer.loop"))
                    {
                        response = "You dont have perms to do that. Not enough perms: audioplayer.loop";
                        return false;
                    }
                    if (arguments.Count == 1 || arguments.Count == 2)
                    {
                        response = "Usage: audio loop {Bot ID} {false/true}";
                        return false;
                    }
                    int id = int.Parse(arguments.At(1));
                    if (plugin.FakeConnectionsIds.TryGetValue(id, out FakeConnectionList hub))
                        hub.audioplayer.Loop = Convert.ToBoolean(arguments.At(1));
                }
                break;
        }
        response = "Done";
        return true;
    }
}