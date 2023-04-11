using Exiled.API.Features;
using Exiled.Events.EventArgs.Interfaces;

namespace AudioPlayer.Other.EventsArgs
{
    public class AudioElevatorFinishedEventArgs : IPlayerEvent
    {
        public Player Player { get; }
        public Lift Lift { get; }
        public BotsList BotsList { get; }
        public string Path { get; }

        public AudioElevatorFinishedEventArgs(Player player, Lift elevator, BotsList botslist, string path)
        {
            Player = player;
            Lift = elevator;
            BotsList = botslist;
            Path = path;
        }
    }
}

