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

    static void OnRoundStarted() => Extensions.PlayRandomAudioFile(Instance.Config.RoundStartClip, "RoundStartClip");

    static void OnRoundEnded(RoundEndedEventArgs ev) => Extensions.PlayRandomAudioFile(Instance.Config.RoundEndClip, "RoundEndClip");

    static void OnVerified(VerifiedEventArgs ev) => Extensions.PlayRandomAudioFileFromPlayer(Instance.Config.PlayerConnectedServer, ev.Player, "PlayerConnectedServer");

    static void OnAnnouncingNtfEntrance(AnnouncingNtfEntranceEventArgs ev) => ev.IsAllowed = Instance.Config.CassieMtfSpawn;

    static void OnDied(DiedEventArgs ev)
    {
        if (ev.Player == null || ev.Attacker == null || ev.DamageHandler.Type == Exiled.API.Enums.DamageType.Unknown) return;

        Extensions.PlayRandomAudioFileFromPlayer(Instance.Config.PlayerDiedTargetClip, ev.Player, "PlayerDiedTargetClip");
        Extensions.PlayRandomAudioFileFromPlayer(Instance.Config.PlayerDiedKillerClip, ev.Attacker, "PlayerDiedKillerClip");
    }

    static void OnRespawningTeam(RespawningTeamEventArgs ev)
    {
        if (ev.NextKnownTeam == Faction.FoundationStaff)
            Extensions.PlayRandomAudioFile(Instance.Config.ChaosSpawnClip, "ChaosSpawnClip");
        else
            Extensions.PlayRandomAudioFile(Instance.Config.MtfSpawnClip, "MtfSpawnClip");
    }
}