using Exiled.API.Features;
using Exiled.Events.EventArgs.Warhead;
using System.Collections.Generic;
using static AudioPlayer.Plugin;

namespace AudioPlayer.Other.DLC;

internal class WarheadEvents
{
    public WarheadEvents()
    {
        Exiled.Events.Handlers.Warhead.Starting += OnWarheadStarting;
        Exiled.Events.Handlers.Warhead.Stopping += OnWarheadStopping;
        Exiled.Events.Handlers.Warhead.Detonated += OnWarheadDetonated;
    }

    static AudioFile currentAudioFile;

    static void OnWarheadStarting(StartingEventArgs ev)
    {
        if (!Warhead.CanBeStarted)
        {
            return;
        }

        WarheadSoundControl(Instance.Config.WarheadStartingClip, "WarheadStartingClip");
    }

    static void OnWarheadDetonated()
    {
        currentAudioFile?.Stop();
    }

    static void OnWarheadStopping(StoppingEventArgs ev)
    {
        if (Instance.Config.WarheadStopping)
        {
            currentAudioFile?.Stop();
        }

        WarheadSoundControl(Instance.Config.WarheadStoppingClip, "WarheadStoppingClip");
    }

    static void WarheadSoundControl(List<AudioFile> audiolist, string audioClipsName)
    {
        currentAudioFile = Extensions.PlayRandomAudioFile(audiolist, audioClipsName);
    }
}