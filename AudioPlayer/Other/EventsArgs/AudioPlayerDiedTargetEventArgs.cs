using Exiled.API.Features;
using Exiled.Events.EventArgs.Interfaces;

namespace AudioPlayer.Other.EventsArgs
{
    public class AudioPlayerDiedTargetEventArgs : IPlayerEvent
    {
        public Player Player { get; }
        public string Path { get; }
        public BotsList BotsList { get; }

        public AudioPlayerDiedTargetEventArgs(Player player, BotsList botslist, string path)
        {
            Player = player;
            BotsList = botslist;
            Path = path;
        }
    }
}
