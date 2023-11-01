using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using Exiled.Events.EventArgs.Warhead;
using Respawning;
using static AudioPlayer.Plugin;

namespace AudioPlayer.Other.DLC;

internal class SpecialEvents
{
    internal static int WarheadStartBotId = 0;
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
    // Stole the code from the old AudioPlayer :jermasus:
    internal void OnRoundStarted() => Extensions.PlayRandomAudioFile(plugin.Config.RoundStartClip);
    internal void OnRoundEnded(RoundEndedEventArgs ev) => Extensions.PlayRandomAudioFile(plugin.Config.RoundEndClip);
    internal void OnVerified(VerifiedEventArgs ev) => Extensions.PlayRandomAudioFileFromPlayer(plugin.Config.PlayerConnectedServer, ev.Player);
    internal void OnAnnouncingNtfEntrance(AnnouncingNtfEntranceEventArgs ev) => ev.IsAllowed = plugin.Config.CassieMtfSpawn;
    internal void OnWarheadStarting(StartingEventArgs ev) => Extensions.WarheadSoundControl(Extensions.PlayRandomAudioFile(plugin.Config.MtfSpawnClip).BotId, false, true); // Getting a bot ID through a song, lol
    internal void OnWarheadDetonated() => Extensions.WarheadSoundControl(0);
    internal void OnWarheadStopping(StoppingEventArgs ev) => Extensions.WarheadSoundControl(0, audiolist: plugin.Config.WarheadStoppingClip);
    internal void OnDied(DiedEventArgs ev)
    {
        if (ev.Player == null || ev.Attacker == null || ev.DamageHandler.Type == Exiled.API.Enums.DamageType.Unknown) return;
        Extensions.PlayRandomAudioFileFromPlayer(plugin.Config.PlayerDiedTargetClip, ev.Player);
        Extensions.PlayRandomAudioFileFromPlayer(plugin.Config.PlayerDiedKillerClip, ev.Attacker);
    }

    internal void OnRespawningTeam(RespawningTeamEventArgs ev)
    {
        if (ev.NextKnownTeam == SpawnableTeamType.ChaosInsurgency) 
            Extensions.PlayRandomAudioFile(plugin.Config.ChaosSpawnClip);
        else Extensions.PlayRandomAudioFile(plugin.Config.MtfSpawnClip);
    }
}
