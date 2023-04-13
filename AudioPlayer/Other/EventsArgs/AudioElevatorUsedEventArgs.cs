using Exiled.API.Features;
using Exiled.Events.EventArgs.Interfaces;

namespace AudioPlayer.Other.EventsArgs
{
    public class AudioElevatorUsedEventArgs : IPlayerEvent
    {
        public Player Player { get; }
        public Lift Lift { get; }
        public BotsList BotsList { get; }
        public string Path { get; }

        public AudioElevatorUsedEventArgs(Player player, Lift elevator, BotsList botslist, string path)
        {
            Player = player;
            Lift = elevator;
            BotsList = botslist;
            Path = path;
        }
    }
}
