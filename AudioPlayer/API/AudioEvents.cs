namespace AudioPlayer.API;

using Exiled.Events.Extensions;
using AudioPlayer.Other.EventsArgs;
using static Exiled.Events.Events;

public static class AudioEvents
{
    public static event CustomEventHandler<AudioPlayerDiedAttackerEventArgs> AudioPlayerDiedAttacker;
    public static event CustomEventHandler<AudioPlayerDiedTargetEventArgs> AudioPlayerDiedTarget;
    public static void OnAudioPlayerDiedAttacker(AudioPlayerDiedAttackerEventArgs ev) => AudioPlayerDiedAttacker.InvokeSafely(ev);
    public static void OnAudioPlayerDiedTarget(AudioPlayerDiedTargetEventArgs ev) => AudioPlayerDiedTarget.InvokeSafely(ev);

}