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
    class Play : ParentCommand
    {
        public override string Command { get; } = "play";

        public override string Description { get; } = "Playing Audio From Path";

        public override string[] Aliases { get; } = { "pl" };

        public override void LoadGeneratedCommands()
        {

        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            if (!sender.CheckPermission("audioplayer.play"))
            {
                response = "You dont have perms to do that";
                return false;
            }

            if (arguments.Count == 0)
            {
                response = "Usage: play path";
                return false;
            }

            string path = string.Join(" ", arguments.ToArray());

            if (!File.Exists(path))
            {
                response = $"No files exist inside that path.\nPath: {path}";
                return false;
            }

            var audioPlayer = AudioPlayerBase.Get(Plugin.plugin.hubPlayer);
            
            audioPlayer.BroadcastChannel = VoiceChatChannel.Intercom;
            audioPlayer.Enqueue(path, -1);
            audioPlayer.Volume = 100;
            audioPlayer.Play(0);

            response = "Playing...";
            return true;
        }
    }
}
