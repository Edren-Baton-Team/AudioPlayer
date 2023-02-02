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
    public class Volume : ICommand, IUsageProvider
    {
        public string Command { get; } = "volume";

        public string Description { get; } = "set up an volume ffor audio";

        public string[] Aliases { get; } = { "vol" };

        public string[] Usage { get; set; } = new string[] { "number" };
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            if (!sender.CheckPermission("audioplayer.volume"))
            {
                response = "You dont have perms to do that";
                return false;
            }

            if (!float.TryParse(arguments.At(0), out float volume))
            {
                response = "Couldn't parse that volume, make sure it is a integer between 0 and 100";
            }
            var audioPlayer = AudioPlayerBase.Get(Plugin.plugin.hubPlayer);
            audioPlayer.Volume = volume;
            response = "Setted up!";
            return true;
        }
    }
}
