using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using SCPSLAudioApi.AudioCore;

namespace AudioPlayer.Other.DLC;

internal class LobbyEvents
{
    public LobbyEvents()
    {
        Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
        Exiled.Events.Handlers.Player.Verified += OnVerified;
        AudioPlayerBase.OnFinishedTrack += OnFinishedTrack;
    }

    static AudioFile currentAudioFile;
    static bool firstPlayerJoinServer;

    static void OnVerified(VerifiedEventArgs ev) // This is done because AudioPlayerBot doesn't have time to log into the server + Why play audio to an empty server?
    {
        if (!firstPlayerJoinServer && !ev.Player.IsNPC && !Round.IsStarted)
        {
            firstPlayerJoinServer = true;
            LobbySoundControl();
        }
    }

    static void OnRoundStarted() => currentAudioFile?.Stop();

    void OnFinishedTrack(AudioPlayerBase playerBase, string track, bool directPlay, ref int nextQueuePos)
    {
        if (!Round.IsLobby)
        {
            UnregisteredEvent();
            return;
        }

        if (currentAudioFile?.Path == track)
        {
            LobbySoundControl();
        }
    }

    public void UnregisteredEvent()
    {
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
        Exiled.Events.Handlers.Player.Verified -= OnVerified;
        AudioPlayerBase.OnFinishedTrack -= OnFinishedTrack;
        currentAudioFile = null;
        Plugin.LobbyEvents = null;
    }

    static void LobbySoundControl()
    {
        currentAudioFile = Extensions.PlayRandomAudioFile(Plugin.Instance.Config.LobbyPlaylist, "LobbyPlaylist");
    }
}