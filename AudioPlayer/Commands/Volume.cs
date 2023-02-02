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
    class volume : ParentCommand
    {
        public override string Command { get; } = "volume";

        public override string Description { get; } = "set up an volume ffor audio";

        public override string[] Aliases { get; } = { "vol" };

        public override void LoadGeneratedCommands()
        {

        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
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
