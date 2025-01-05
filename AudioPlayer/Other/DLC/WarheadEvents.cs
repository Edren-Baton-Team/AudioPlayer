using Exiled.API.Features;
using Exiled.Events.EventArgs.Warhead;
using System.Collections.Generic;
using static AudioPlayer.Plugin;

namespace AudioPlayer.Other.DLC;

internal class WarheadEvents
{
    private static AudioFile CurrentAudioFile = null;
    public WarheadEvents()
    {
        Exiled.Events.Handlers.Warhead.Starting += OnWarheadStarting;
        Exiled.Events.Handlers.Warhead.Stopping += OnWarheadStopping;
        Exiled.Events.Handlers.Warhead.Detonated += OnWarheadDetonated;
    }

    void OnWarheadStarting(StartingEventArgs ev)
    {
        if (!Warhead.CanBeStarted)
        {
            return;
        }

        WarheadSoundControl(plugin.Config.WarheadStartingClip, "WarheadStartingClip");
    }

    void OnWarheadDetonated()
    {
        CurrentAudioFile?.Stop();
    }

    void OnWarheadStopping(StoppingEventArgs ev)
    {
        if (plugin.Config.WarheadStopping)
        {
            CurrentAudioFile?.Stop();
        }

        WarheadSoundControl(plugin.Config.WarheadStoppingClip, "WarheadStoppingClip");
    }

    static void WarheadSoundControl(List<AudioFile> audiolist, string audioClipsName)
    {
        CurrentAudioFile = Extensions.PlayRandomAudioFile(audiolist, audioClipsName);
    }
}
