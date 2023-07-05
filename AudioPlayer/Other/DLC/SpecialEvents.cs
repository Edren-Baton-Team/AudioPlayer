using AudioPlayer.Other.EventsArgs;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using Exiled.Events.EventArgs.Warhead;
using Respawning;
using System.Collections.Generic;
using UnityEngine;
using static AudioPlayer.Plugin;

namespace AudioPlayer.Other.DLC
{
    internal class SpecialEvents
    {
        private int WarheadStartBotId = 0;
        // Stole the code from the old AudioPlayer :jermasus:
        internal void OnRoundStarted()
        {
            List<AudioFile> playlist = plugin.Config.RoundStartClip;

            if (playlist.Count > 0)
                playlist[Random.Range(0, playlist.Count)].Play();
        }
        internal void OnRoundEnded(RoundEndedEventArgs ev)
        {
            List<AudioFile> playlist = plugin.Config.RoundEndClip;

            if (playlist.Count > 0)
                playlist[Random.Range(0, playlist.Count)].Play();
        }
        internal void OnRespawningTeam(RespawningTeamEventArgs ev)
        {
            switch (ev.NextKnownTeam)
            {
                case SpawnableTeamType.ChaosInsurgency:
                    {
                        List<AudioFile> playlist = plugin.Config.ChaosSpawnClip;

                        if (playlist.Count > 0)
                            playlist[Random.Range(0, playlist.Count)].Play();
                    }
                    break;
                case SpawnableTeamType.NineTailedFox:
                    {
                        List<AudioFile> playlist1 = plugin.Config.MtfSpawnClip;

                        if (playlist1.Count > 0)
                            playlist1[Random.Range(0, playlist1.Count)].Play();
                    }
                    break;
            }
        }

        internal void OnWarheadStarting(StartingEventArgs ev)
        {
            if (!Warhead.CanBeStarted)
                return;

            AudioFile playlist = plugin.Config.WarheadStartingClip[Random.Range(0, plugin.Config.WarheadStartingClip.Count)];

            if (plugin.Config.WarheadStartingClip.Count > 0)
            {
                if (plugin.Config.WarheadStopping)
                    WarheadStartBotId = playlist.BotId;
                playlist.Play();
            }
        }


        internal void OnWarheadStopping(StoppingEventArgs ev)
        {
            List<AudioFile> playlist = plugin.Config.WarheadStoppingClip;

            if (plugin.Config.WarheadStopping)
            {
                API.AudioController.StopAudio(WarheadStartBotId);
                WarheadStartBotId = 0;
            }
            if (playlist.Count > 0)
                playlist[Random.Range(0, playlist.Count)].Play();
        }
        internal void OnVerified(VerifiedEventArgs ev)
        {
            List<AudioFile> playlist = plugin.Config.PlayerConnectedServer;

            if (playlist.Count > 0)
                playlist[Random.Range(0, playlist.Count)].PlayFromFilePlayer(new List<int>() { ev.Player.Id });
        }

        internal void OnDied(DiedEventArgs ev1)
        {
            if (ev1.Player == null || ev1.Attacker == null || ev1.DamageHandler.Type == Exiled.API.Enums.DamageType.Unknown)
                return;
            AudioFile playlist = plugin.Config.PlayerDiedTargetClip[Random.Range(0, plugin.Config.PlayerDiedTargetClip.Count)];
            AudioFile playlist1 = plugin.Config.PlayerDiedKillerClip[Random.Range(0, plugin.Config.PlayerDiedKillerClip.Count)];
            if (plugin.Config.PlayerDiedTargetClip.Count > 0)
            {
                playlist.PlayFromFilePlayer(new List<int>() { ev1.Player.Id });
                AudioPlayerDiedTargetEventArgs ev = new(ev1.Player, BotsList.Get(playlist.BotId), playlist.Path);
                API.AudioEvents.OnAudioPlayerDiedTarget(ev);
            }
            if (plugin.Config.PlayerDiedKillerClip.Count > 0)
            {
                playlist1.PlayFromFilePlayer(new List<int>() { ev1.Attacker.Id });
                AudioPlayerDiedAttackerEventArgs ev = new(ev1.Player, BotsList.Get(playlist1.BotId), playlist1.Path);
                API.AudioEvents.OnAudioPlayerDiedAttacker(ev);
            }
        }

        internal void OnWarheadDetonated()
        {
            if (plugin.Config.WarheadStopping)
            {
                API.AudioController.StopAudio(WarheadStartBotId);
                WarheadStartBotId = 0;
            }
        }

        internal void OnAnnouncingNtfEntrance(AnnouncingNtfEntranceEventArgs ev)
        {
            ev.IsAllowed = plugin.Config.CassieMtfSpawn;
        }
    }
}
