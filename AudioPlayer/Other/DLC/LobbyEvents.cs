using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using SCPSLAudioApi.AudioCore;

namespace AudioPlayer.Other.DLC;

internal class LobbyEvents
{
    private static AudioFile CurrentAudioFile = null;
    private static bool FirstPlayerJoinServer = false;

    public LobbyEvents()
    {
        Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
        Exiled.Events.Handlers.Player.Verified += OnVerified;
        AudioPlayerBase.OnFinishedTrack += OnFinishedTrack;
    }

    void OnVerified(VerifiedEventArgs ev) // This is done because AudioPlayerBot doesn't have time to log into the server + Why play audio to an empty server?
    {
        if (!FirstPlayerJoinServer && !ev.Player.IsNPC && !Round.IsStarted)
        {
            FirstPlayerJoinServer = true;
            LobbySoundControl();
        }
    }

    void OnRoundStarted() => CurrentAudioFile?.Stop();

    void OnFinishedTrack(AudioPlayerBase playerBase, string track, bool directPlay, ref int nextQueuePos)
    {
        if (!Round.IsLobby)
        {
            UnregisteredEvent();
            return;
        }

        if (CurrentAudioFile?.Path == track)
        {
            LobbySoundControl();
        }
    }

    public void UnregisteredEvent()
    {
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
        Exiled.Events.Handlers.Player.Verified -= OnVerified;
        AudioPlayerBase.OnFinishedTrack -= OnFinishedTrack;
        CurrentAudioFile = null;
        Plugin.LobbyEvents = null;
    }

    static void LobbySoundControl()
    {
        CurrentAudioFile = Extensions.PlayRandomAudioFile(Plugin.plugin.Config.LobbyPlaylist, "LobbyPlaylist");
    }
}
