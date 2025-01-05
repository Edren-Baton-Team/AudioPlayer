using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using PlayerRoles;
using static AudioPlayer.Plugin;

namespace AudioPlayer.Other.DLC;

internal class SpecialEvents
{
    public SpecialEvents()
    {
        Exiled.Events.Handlers.Player.Died += OnDied;
        Exiled.Events.Handlers.Player.Verified += OnVerified;

        Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
        Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
        Exiled.Events.Handlers.Server.RespawningTeam += OnRespawningTeam;

        Exiled.Events.Handlers.Map.AnnouncingNtfEntrance += OnAnnouncingNtfEntrance;
    }

    void OnRoundStarted() => Extensions.PlayRandomAudioFile(plugin.Config.RoundStartClip, "RoundStartClip");
    
    void OnRoundEnded(RoundEndedEventArgs ev) => Extensions.PlayRandomAudioFile(plugin.Config.RoundEndClip, "RoundEndClip");
    
    void OnVerified(VerifiedEventArgs ev) => Extensions.PlayRandomAudioFileFromPlayer(plugin.Config.PlayerConnectedServer, ev.Player, "PlayerConnectedServer");
    
    void OnAnnouncingNtfEntrance(AnnouncingNtfEntranceEventArgs ev) => ev.IsAllowed = plugin.Config.CassieMtfSpawn;
    
    void OnDied(DiedEventArgs ev)
    {
        if (ev.Player == null || ev.Attacker == null || ev.DamageHandler.Type == Exiled.API.Enums.DamageType.Unknown) return;

        Extensions.PlayRandomAudioFileFromPlayer(plugin.Config.PlayerDiedTargetClip, ev.Player, "PlayerDiedTargetClip");
        Extensions.PlayRandomAudioFileFromPlayer(plugin.Config.PlayerDiedKillerClip, ev.Attacker, "PlayerDiedKillerClip");
    }

    void OnRespawningTeam(RespawningTeamEventArgs ev)
    {
        if (ev.NextKnownTeam == Faction.FoundationStaff) 
            Extensions.PlayRandomAudioFile(plugin.Config.ChaosSpawnClip, "ChaosSpawnClip");
        else 
            Extensions.PlayRandomAudioFile(plugin.Config.MtfSpawnClip, "MtfSpawnClip");
    }
}
