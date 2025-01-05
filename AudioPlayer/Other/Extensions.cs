using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils.NonAllocLINQ;
using static AudioPlayer.Plugin;

namespace AudioPlayer.Other;
public static class Extensions
{
    internal static void CreateDirectory()
    {
        if (Directory.Exists(plugin.AudioPath))
        {
            return;
        }

        Directory.CreateDirectory(plugin.AudioPath);
    }

    public static AudioFile EmptyClip;

    public static AudioFile GetRandomAudioClip(List<AudioFile> audioClips, string audioClipsName)
    {
        if (audioClips == null)
        {
            if (plugin.Config.Debug)
            {
                throw new ArgumentException($"{audioClipsName} is null");
            }
            else
            {
                return EmptyClip;
            }
        }

        foreach (AudioFile audioFile in audioClips.Where(clip => !AudioPlayerList.ContainsKey(clip.BotId)).ToArray())
        {
            audioClips.Remove(audioFile);
            Log.Debug($"Removed AudioClip from {audioClipsName} because AudioPlayerBot is not present on the server");
        }

        if (!audioClips.Any())
        {
            if (plugin.Config.Debug)
            {
                throw new ArgumentException($"I didn't find any available AudioClips in {audioClipsName}, maybe you didn't specify AudioPlayerBot in the config or didn't spawn an AudioPlayerBot");
            }
            else
            {
                return EmptyClip;
            }
        }

        return audioClips.RandomItem();
    }

    public static AudioFile PlayRandomAudioFile(List<AudioFile> audioClips, string audioClipsName = "")
    {
        AudioFile randomClip = GetRandomAudioClip(audioClips, audioClipsName);
        randomClip.Play();

        return randomClip;
    }

    public static AudioFile PlayRandomAudioFileFromPlayer(List<AudioFile> audioClips, Player player, string audioClipsName = "")
    {
        AudioFile randomClip = GetRandomAudioClip(audioClips, audioClipsName);
        randomClip.PlayFromFilePlayer([player.Id]);

        return randomClip;
    }

    public static string PathCheck(string path)
    {
        if (File.Exists(path))
        {
            Log.Debug("Full path was specified, skipping the check.");
            return path;
        }
        else if (File.Exists(Path.Combine(plugin.AudioPath, path)))
        {
            path = Path.Combine(plugin.AudioPath, path);
            Log.Debug("An incomplete path was given, I found the .ogg file in the audio folder.");
            return path;
        }
        else
        {
            Log.Debug($"I didn't find the file.\nPath: {path}");
            return path;
        }
    }
}