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
    class stop : ParentCommand
    {
        public override string Command { get; } = "stop";

        public override string Description { get; } = "stops musics";

        public override string[] Aliases { get; } = { "st" };

        public override void LoadGeneratedCommands()
        {

        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
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
