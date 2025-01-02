using AudioPlayer.API;
using AudioPlayer.API.Container;
using Exiled.API.Features;
using MEC;
using Mirror;
using PlayerRoles;
using SCPSLAudioApi.AudioCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Utils.NonAllocLINQ;
using static AudioPlayer.Plugin;
using Object = UnityEngine.Object;

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
    internal static void WarheadSoundControl(int botId, bool stopSong = true, bool CanBeStartedWarhead = false, List<AudioFile> audiolist = null)
    {
        if (!plugin.Config.WarheadStopping)
        {
            return;
        }

        if (stopSong)
        {
            AudioController.TryGetAudioPlayerContainer(DLC.SpecialEvents.WarheadStartBotId).StopAudio();
        }

        if (!Warhead.CanBeStarted && CanBeStartedWarhead)
        {
            return;
        }

        DLC.SpecialEvents.WarheadStartBotId = botId;

        if (audiolist != null)
        {
            PlayRandomAudioFile(plugin.Config.WarheadStoppingClip);
        }
    }
    
    // Переписать
    public static AudioFile PlayRandomAudioFile(List<AudioFile> audioClip, bool lobbyPlaylist = false, bool noBaseEvent = false)
    {
        if (audioClip == null) // Solves two problems, the first is that I don't have to write lobbysong every time. The second is that there may never be a null value here :troll:
        {
            audioClip = plugin.Config.LobbyPlaylist;
        }

        if ((!noBaseEvent && !plugin.Config.SpecialEventsEnable) || audioClip.Count == 0)
        {
            return null;
        }

        var randomClip = audioClip.RandomItem();
        randomClip.Play();

        if (lobbyPlaylist && AudioController.GetAllGetAudioPlayer().Any() && Round.IsLobby)
        {
            plugin.LobbySong = randomClip;
        }

        return randomClip;
    }
    public static AudioFile PlayRandomAudioFileFromPlayer(List<AudioFile> audioClip, Player player, bool noBaseEvent = false)
    {
        if (audioClip == null)
        {
            audioClip = plugin.Config.LobbyPlaylist;
        }

        if ((!noBaseEvent && !plugin.Config.SpecialEventsEnable) || audioClip.Count == 0)
        {
            return null;
        }

        var randomClip = audioClip.RandomItem();
        randomClip.PlayFromFilePlayer(new List<int>() { player.Id });

        return randomClip;
    }

    public static AudioPlayerBot SpawnDummy(string name = "Dedicated Server", string badgetext = "AudioPlayer BOT", string bagdecolor = "orange", int id = 99, RoleTypeId roleTypeId = RoleTypeId.Overwatch)
    {
        if (AudioController.IsAudioPlayer(id))
        {
            Log.Error("This id is already in use");
            return null;
        }

        var npc = Npc.Spawn(name, roleTypeId);
        npc.ReferenceHub.nicknameSync.Network_myNickSync = name;

        var container = new AudioPlayerBot(id, name, AudioPlayerBase.Get(npc.ReferenceHub), npc);

        AudioPlayerList.Add(id, container);

        npc.RankName = badgetext;
        npc.RankColor = bagdecolor;

        return container;
    }
}