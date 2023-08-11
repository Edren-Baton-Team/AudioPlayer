namespace AudioPlayer.API;

using AudioPlayer.Other.EventsArgs;
using Exiled.Events.Features;

public static class AudioEvents
{
    public static Event<AudioPlayerDiedAttackerEventArgs> AudioPlayerDiedAttacker { get; set; } = new();
    public static Event<AudioPlayerDiedTargetEventArgs> AudioPlayerDiedTarget { get; set; } = new();
    public static void OnAudioPlayerDiedAttacker(AudioPlayerDiedAttackerEventArgs ev) => AudioPlayerDiedAttacker.InvokeSafely(ev);
    public static void OnAudioPlayerDiedTarget(AudioPlayerDiedTargetEventArgs ev) => AudioPlayerDiedTarget.InvokeSafely(ev);

}