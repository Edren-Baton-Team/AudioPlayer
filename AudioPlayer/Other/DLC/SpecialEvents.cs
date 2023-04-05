using AudioPlayer.Other.EventsArgs;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using Exiled.Events.EventArgs.Warhead;
using MEC;
using Respawning;
using System.Collections.Generic;


namespace AudioPlayer.Other.DLC
{
    public class SpecialEvents
    {
        private List<AudioFile> ElevatorClip = Plugin.plugin.Config.ElevatorClip;
        private List<int> ElevatorListPlayer = new List<int>();
        // Stole the code from the old AudioPlayer :jermasus:
        public void OnRoundStarted()
        {
            List<AudioFile> playlist = Plugin.plugin.Config.RoundStartClip;

            if (playlist.Count > 0)
                playlist[UnityEngine.Random.Range(0, playlist.Count)].Play();
        }
        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            List<AudioFile> playlist = Plugin.plugin.Config.RoundEndClip;

            if (playlist.Count > 0)
                playlist[UnityEngine.Random.Range(0, playlist.Count)].Play();
        }
        public void OnRespawningTeam(RespawningTeamEventArgs ev)
        {
            switch (ev.NextKnownTeam)
            {
                case SpawnableTeamType.ChaosInsurgency:
                    List<AudioFile> playlist = Plugin.plugin.Config.ChaosSpawnClip;

                    if (playlist.Count > 0)
                        playlist[UnityEngine.Random.Range(0, playlist.Count)].Play();
                    break;
                case SpawnableTeamType.NineTailedFox:
                    List<AudioFile> playlist1 = Plugin.plugin.Config.MtfSpawnClip;

                    if (playlist1.Count > 0)
                        playlist1[UnityEngine.Random.Range(0, playlist1.Count)].Play();
                    break;
            }
        }

        public void OnWarheadStarting(StartingEventArgs ev)
        {
            List<AudioFile> playlist = Plugin.plugin.Config.WarheadStartingClip;

            if (playlist.Count > 0)
                playlist[UnityEngine.Random.Range(0, playlist.Count)].Play();
        }


        public void OnWarheadStopping(StoppingEventArgs ev)
        {
            List<AudioFile> playlist = Plugin.plugin.Config.WarheadStoppingClip;

            if (playlist.Count > 0)
                playlist[UnityEngine.Random.Range(0, playlist.Count)].Play();
        }
        public void OnVerified(VerifiedEventArgs ev)
        {
            Timing.CallDelayed(0.5f, () => //Yes, I love timings. 
            {
                if (ev.Player.IsAlive)
                {
                    Log.Debug("Add player ElevatorPlayerStatus");
                    Timing.RunCoroutine(ElevatorPlayerStatus(ev.Player));
                }
            });
            List<AudioFile> playlist = Plugin.plugin.Config.PlayerConnectedServer;

            if (playlist.Count > 0)
                playlist[UnityEngine.Random.Range(0, playlist.Count)].PlayFromFilePlayer(new List<int>() { ev.Player.Id });
        }
        public IEnumerator<float> ElevatorPlayerStatus(Player ply)
        {
            for (; ; )
            {
                yield return Timing.WaitForSeconds(0.5f);
                if (ply.Lift == null)
                {
                    continue;
                }
                if (ply.Lift.Status == Interactables.Interobjects.ElevatorChamber.ElevatorSequence.DoorClosing)
                {
                    if (ElevatorClip.Count > 0)
                    {
                        ElevatorListPlayer.Add(ply.Id);
                        ElevatorClip[UnityEngine.Random.Range(0, ElevatorClip.Count)].PlayFromFilePlayer(ElevatorListPlayer);
                        foreach (BotsList bots in Plugin.plugin.Config.BotsList)
                        {
                            AudioElevatorStartEventArgs ev = new(ply, ply.Lift, bots, ElevatorClip[UnityEngine.Random.Range(0, ElevatorClip.Count)].Path);
                        }
                    }
                    Log.Debug("Start Elevator Music");

                    yield return Timing.WaitForSeconds(2);
                }
                if (ply.Lift.Status == Interactables.Interobjects.ElevatorChamber.ElevatorSequence.MovingAway)
                {
                    if (ElevatorClip.Count > 0)
                        foreach (BotsList bots in Plugin.plugin.Config.BotsList)
                        {
                            AudioElevatorUsedEventArgs ev = new(ply, ply.Lift, bots, ElevatorClip[UnityEngine.Random.Range(0, ElevatorClip.Count)].Path);
                        }
                    Log.Debug("Start Elevator MovingAway");
                }
                if (ply.Lift.Status == Interactables.Interobjects.ElevatorChamber.ElevatorSequence.DoorOpening)
                {
                    if (ElevatorClip.Count > 0)
                    {
                        ElevatorListPlayer.Remove(ply.Id);
                        ElevatorClip[UnityEngine.Random.Range(0, ElevatorClip.Count)].Stop();
                        foreach (BotsList bots in Plugin.plugin.Config.BotsList)
                        {
                            AudioElevatorFinishedEventArgs ev = new(ply, ply.Lift, bots, ElevatorClip[UnityEngine.Random.Range(0, ElevatorClip.Count)].Path);
                        }
                    }
                    Log.Debug("Start Elevator DoorOpening");
                }
            }
        }

        public void OnDied(DiedEventArgs ev1)
        {
            if (ev1.Player == null || ev1.Attacker == null || ev1.DamageHandler.Type == Exiled.API.Enums.DamageType.Unknown)
                return;
            List<AudioFile> playlist = Plugin.plugin.Config.PlayerDiedTargetClip;
            List<AudioFile> playlist1 = Plugin.plugin.Config.PlayerDiedKillerClip;
            if (playlist.Count > 0)
                foreach (BotsList bots in Plugin.plugin.Config.BotsList)
                {
                    playlist[UnityEngine.Random.Range(0, playlist.Count)].PlayFromFilePlayer(new List<int>() { ev1.Player.Id });
                    AudioPlayerDiedTargetEventArgs ev = new(ev1.Player, bots, playlist[UnityEngine.Random.Range(0, playlist.Count)].Path);
                }
            if (playlist1.Count > 0)
                foreach (BotsList bots in Plugin.plugin.Config.BotsList)
                {
                    playlist1[UnityEngine.Random.Range(0, playlist1.Count)].PlayFromFilePlayer(new List<int>() { ev1.Attacker.Id });
                    AudioPlayerDiedAttackerEventArgs ev = new(ev1.Player, bots, playlist1[UnityEngine.Random.Range(0, playlist1.Count)].Path);
                }
        }
    }
}
