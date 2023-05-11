using Exiled.API.Features;
using Exiled.Events.EventArgs.Interfaces;

namespace AudioPlayer.Other.EventsArgs
{
    public class AudioPlayerDiedAttackerEventArgs : IPlayerEvent
    {
        public Player Player { get; }
        public string Path { get; }
        public BotsList BotsList { get; }

        public AudioPlayerDiedAttackerEventArgs(Player player, BotsList botslist, string path)
        {
            Player = player;
            BotsList = botslist;
            Path = path;
        }
    }
}
