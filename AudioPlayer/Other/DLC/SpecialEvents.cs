using AudioPlayer.Other.EventsArgs;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using Exiled.Events.EventArgs.Warhead;
using MEC;
using Respawning;
using System.Collections.Generic;
using UnityEngine;
using static AudioPlayer.Plugin;


namespace AudioPlayer.Other.DLC
{
    public class SpecialEvents
    {
        private List<AudioFile> ElevatorClip = plugin.Config.ElevatorClip;
        private List<int> ElevatorListPlayer = new();
        private int WarheadStartBotId = 0;
        // Stole the code from the old AudioPlayer :jermasus:
        public void OnRoundStarted()
        {
            List<AudioFile> playlist = plugin.Config.RoundStartClip;

            if (playlist.Count > 0)
                playlist[Random.Range(0, playlist.Count)].Play();
        }
        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            List<AudioFile> playlist = plugin.Config.RoundEndClip;

            if (playlist.Count > 0)
                playlist[Random.Range(0, playlist.Count)].Play();
        }
        public void OnRespawningTeam(RespawningTeamEventArgs ev)
        {
            switch (ev.NextKnownTeam)
            {
                case SpawnableTeamType.ChaosInsurgency:
                    List<AudioFile> playlist = plugin.Config.ChaosSpawnClip;

                    if (playlist.Count > 0)
                        playlist[Random.Range(0, playlist.Count)].Play();
                    break;
                case SpawnableTeamType.NineTailedFox:
                    List<AudioFile> playlist1 = plugin.Config.MtfSpawnClip;

                    if (playlist1.Count > 0)
                        playlist1[Random.Range(0, playlist1.Count)].Play();
                    break;
            }
        }

        public void OnWarheadStarting(StartingEventArgs ev)
        {
            List<AudioFile> playlist = plugin.Config.WarheadStartingClip;

            if (playlist.Count > 0)
            {
                int random = Random.Range(0, playlist.Count);
                if (plugin.Config.WarheadStopping)
                    WarheadStartBotId = playlist[random].BotID;
                playlist[random].Play();
            }
        }


        public void OnWarheadStopping(StoppingEventArgs ev)
        {
            List<AudioFile> playlist = plugin.Config.WarheadStoppingClip;

            if (plugin.Config.WarheadStopping)
            {
                API.AudioController.StopAudio(WarheadStartBotId);
                return;
            }
            if (playlist.Count > 0)
                playlist[Random.Range(0, playlist.Count)].Play();
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
            List<AudioFile> playlist = plugin.Config.PlayerConnectedServer;

            if (playlist.Count > 0)
                playlist[Random.Range(0, playlist.Count)].PlayFromFilePlayer(new List<int>() { ev.Player.Id });
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
                        ElevatorClip[Random.Range(0, ElevatorClip.Count)].PlayFromFilePlayer(ElevatorListPlayer);
                        foreach (BotsList bots in plugin.Config.BotsList)
                        {
                            AudioElevatorStartEventArgs ev = new(ply, ply.Lift, bots, ElevatorClip[Random.Range(0, ElevatorClip.Count)].Path);
                        }
                    }
                    Log.Debug("Start Elevator Music");

                    yield return Timing.WaitForSeconds(2);
                }
                if (ply.Lift.Status == Interactables.Interobjects.ElevatorChamber.ElevatorSequence.MovingAway)
                {
                    if (ElevatorClip.Count > 0)
                        foreach (BotsList bots in plugin.Config.BotsList)
                        {
                            AudioElevatorUsedEventArgs ev = new(ply, ply.Lift, bots, ElevatorClip[Random.Range(0, ElevatorClip.Count)].Path);
                        }
                    Log.Debug("Start Elevator MovingAway");
                }
                if (ply.Lift.Status == Interactables.Interobjects.ElevatorChamber.ElevatorSequence.DoorOpening)
                {
                    if (ElevatorClip.Count > 0)
                    {
                        ElevatorListPlayer.Remove(ply.Id);
                        ElevatorClip[Random.Range(0, ElevatorClip.Count)].Stop();
                        foreach (BotsList bots in plugin.Config.BotsList)
                        {
                            AudioElevatorFinishedEventArgs ev = new(ply, ply.Lift, bots, ElevatorClip[Random.Range(0, ElevatorClip.Count)].Path);
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
            List<AudioFile> playlist = plugin.Config.PlayerDiedTargetClip;
            List<AudioFile> playlist1 = plugin.Config.PlayerDiedKillerClip;
            if (playlist.Count > 0)
                foreach (BotsList bots in plugin.Config.BotsList)
                {
                    playlist[Random.Range(0, playlist.Count)].PlayFromFilePlayer(new List<int>() { ev1.Player.Id });
                    AudioPlayerDiedTargetEventArgs ev = new(ev1.Player, bots, playlist[Random.Range(0, playlist.Count)].Path);
                }
            if (playlist1.Count > 0)
                foreach (BotsList bots in plugin.Config.BotsList)
                {
                    playlist1[Random.Range(0, playlist1.Count)].PlayFromFilePlayer(new List<int>() { ev1.Attacker.Id });
                    AudioPlayerDiedAttackerEventArgs ev = new(ev1.Player, bots, playlist1[Random.Range(0, playlist1.Count)].Path);
                }
        }
    }
}
