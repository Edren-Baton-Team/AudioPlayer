using System;
using System.IO;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using SCPSLAudioApi.AudioCore;
using VoiceChat;

namespace AudioPlayer.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Stop : ICommand
    {
        public string Command { get; } = "stop";

        public string Description { get; } = "stops musics";

        public string[] Aliases { get; } = { "st" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            if (!sender.CheckPermission("audioplayer.stop"))
            {
                response = "You dont have perms to do that";
                return false;
            }
            var audioPlayer = AudioPlayerBase.Get(Plugin.plugin.hubPlayer);
            audioPlayer.Stoptrack(true);
            response = "Stop";
            return true;
        }
    }
}
