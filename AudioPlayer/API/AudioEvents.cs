using Exiled.Events;
using Exiled.Events.Extensions;
using AudioPlayer.Other.EventsArgs;

namespace AudioPlayer.API;

public static class AudioEvents
{
    public static event Events.CustomEventHandler<AudioElevatorStartEventArgs> AudioElevatorStart;
    public static event Events.CustomEventHandler<AudioElevatorUsedEventArgs> AudioElevatorUsed;
    public static event Events.CustomEventHandler<AudioElevatorFinishedEventArgs> AudioElevatorFinished;
    public static event Events.CustomEventHandler<AudioPlayerDiedAttackerEventArgs> AudioPlayerDiedAttacker;
    public static event Events.CustomEventHandler<AudioPlayerDiedTargetEventArgs> AudioPlayerDiedTarget;

    public static void OnAudioElevatorStart(AudioElevatorStartEventArgs ev) => AudioElevatorStart.InvokeSafely(ev);
    public static void OnAudioElevatorUsed(AudioElevatorUsedEventArgs ev) => AudioElevatorUsed.InvokeSafely(ev);
    public static void OnAudioElevatorUsed(AudioElevatorFinishedEventArgs ev) => AudioElevatorFinished.InvokeSafely(ev);
    public static void OnAudioPlayerDiedAttacker(AudioPlayerDiedAttackerEventArgs ev) => AudioPlayerDiedAttacker.InvokeSafely(ev);
    public static void OnAudioPlayerDiedTarget(AudioPlayerDiedTargetEventArgs ev) => AudioPlayerDiedTarget.InvokeSafely(ev);

}