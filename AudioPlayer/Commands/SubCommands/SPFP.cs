using AudioPlayer.API;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using System.Collections.Generic;

namespace AudioPlayer.Commands.SubCommands
{
    public class SPFP : ICommand, IUsageProvider
    {
        public string Command => "stopplayfromplayers";

        public string[] Aliases { get; } = { "spfp", "stoppfp" };

        public string Description => "AudioPlayer Bot stop audio playback for certain players";

        public string[] Usage { get; } = { "Bot ID", "PlayerList" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("audioplayer.stoppfp"))
            {
                response = "You dont have perms to do that. Not enough perms: audioplayer.stoppfp";
                return false;
            }
            if (arguments.Count <= 1)
            {
                response = "Usage: audio spfp {Bot ID} {PlayerList}";
                return false;
            }
            int id = int.Parse(arguments.At(0));
            List<int> list = new List<int>();
            string texttoresponse = string.Empty;
            foreach (string s in arguments.At(1).Trim('.').Split('.'))
            {
                var pl = Player.Get(s);
                texttoresponse += pl.Nickname + ", ";
                list.Add(pl.Id);
            }
            AudioController.StopPlayerFromPlaying(list, id);
            response = $"Stopped the sound at ID {id} for the next player: {texttoresponse}";
            return true;
        }
    }
}
