using Exiled.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using Exiled.Events.EventArgs.Warhead;
using Respawning;
using System.Collections.Generic;
using UnityEngine;
using static AudioPlayer.Plugin;

namespace AudioPlayer.Other.DLC;

internal class SpecialEvents
{
    public SpecialEvents()
    {
        Exiled.Events.Handlers.Map.AnnouncingNtfEntrance += OnAnnouncingNtfEntrance;
        Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
        Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
        Exiled.Events.Handlers.Server.RespawningTeam += OnRespawningTeam;
        Exiled.Events.Handlers.Warhead.Starting += OnWarheadStarting;
        Exiled.Events.Handlers.Warhead.Detonated += OnWarheadDetonated;
        Exiled.Events.Handlers.Warhead.Stopping += OnWarheadStopping;
        Exiled.Events.Handlers.Player.Verified += OnVerified;
        Exiled.Events.Handlers.Player.Died += OnDied;
    }
    ~SpecialEvents()
    {
        Exiled.Events.Handlers.Map.AnnouncingNtfEntrance -= OnAnnouncingNtfEntrance;
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
        Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
        Exiled.Events.Handlers.Server.RespawningTeam -= OnRespawningTeam;
        Exiled.Events.Handlers.Warhead.Starting -= OnWarheadStarting;
        Exiled.Events.Handlers.Warhead.Detonated -= OnWarheadDetonated;
        Exiled.Events.Handlers.Warhead.Stopping -= OnWarheadStopping;
        Exiled.Events.Handlers.Player.Verified -= OnVerified;
        Exiled.Events.Handlers.Player.Died -= OnDied;
    }
    private int WarheadStartBotId = 0;
    // Stole the code from the old AudioPlayer :jermasus:
    internal void OnRoundStarted()
    {
        List<AudioFile> playlist = plugin.Config.RoundStartClip;

        if (playlist.Count > 0)
            playlist.RandomItem().Play();
    }
    internal void OnRoundEnded(RoundEndedEventArgs ev)
    {
        List<AudioFile> playlist = plugin.Config.RoundEndClip;

        if (playlist.Count > 0)
            playlist.RandomItem().Play();
    }
    internal void OnRespawningTeam(RespawningTeamEventArgs ev)
    {
        switch (ev.NextKnownTeam)
        {
            case SpawnableTeamType.ChaosInsurgency:
                {
                    List<AudioFile> playlist = plugin.Config.ChaosSpawnClip;

                    if (playlist.Count > 0)
                        playlist.RandomItem().Play();
                }
                break;
            case SpawnableTeamType.NineTailedFox:
                {
                    List<AudioFile> playlist1 = plugin.Config.MtfSpawnClip;

                    if (playlist1.Count > 0)
                        playlist1.RandomItem().Play();
                }
                break;
        }
    }

    internal void OnWarheadStarting(StartingEventArgs ev)
    {
        if (!Warhead.CanBeStarted || plugin.Config.WarheadStartingClip.Count == 0)
            return;

        AudioFile playlist = plugin.Config.WarheadStartingClip.RandomItem();

        if (plugin.Config.WarheadStopping) WarheadStartBotId = playlist.BotId;

        playlist.Play();
    }


    internal void OnWarheadStopping(StoppingEventArgs ev)
    {
        List<AudioFile> playlist = plugin.Config.WarheadStoppingClip;

        if (plugin.Config.WarheadStopping)
        {
            API.AudioController.StopAudio(WarheadStartBotId);
            WarheadStartBotId = 0;
        }
        if (playlist.Count > 0) playlist.RandomItem().Play();
    }
    internal void OnVerified(VerifiedEventArgs ev)
    {
        List<AudioFile> playlist = plugin.Config.PlayerConnectedServer;

        if (playlist.Count > 0)
            playlist.RandomItem().PlayFromFilePlayer(new List<int>() { ev.Player.Id });
    }

    internal void OnDied(DiedEventArgs ev1)
    {
        if (ev1.Player == null || ev1.Attacker == null || ev1.DamageHandler.Type == Exiled.API.Enums.DamageType.Unknown)
            return;
        AudioFile playlist = plugin.Config.PlayerDiedTargetClip.RandomItem();
        AudioFile playlist1 = plugin.Config.PlayerDiedKillerClip.RandomItem();
        if (plugin.Config.PlayerDiedTargetClip.Count > 0)
        {
            playlist.PlayFromFilePlayer(new List<int>() { ev1.Player.Id });
        }
        if (plugin.Config.PlayerDiedKillerClip.Count > 0)
        {
            playlist1.PlayFromFilePlayer(new List<int>() { ev1.Attacker.Id });
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
