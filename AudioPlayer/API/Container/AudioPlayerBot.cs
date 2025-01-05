using AudioPlayer.Other;
using Exiled.API.Features;
using PlayerRoles;
using SCPSLAudioApi.AudioCore;
using System.Collections.Generic;
using VoiceChat;

namespace AudioPlayer.API.Container;

public class AudioPlayerBot
{
    public static AudioPlayerBot SpawnDummy(string name = "Dedicated Server", string badgetext = "AudioPlayer BOT", string bagdecolor = "orange", int id = 99, RoleTypeId roleTypeId = RoleTypeId.Overwatch, bool ignored = true)
    {
        if (AudioController.IsAudioPlayer(id))
        {
            Log.Error("This id is already in use");
            return null;
        }

        var npc = Npc.Spawn(name, roleTypeId, ignored);
        npc.ReferenceHub.nicknameSync.Network_myNickSync = name;

        var container = new AudioPlayerBot(id, name, AudioPlayerBase.Get(npc.ReferenceHub), npc);

        Plugin.AudioPlayerList.Add(id, container);

        npc.RankName = badgetext;
        npc.RankColor = bagdecolor;

        return container;
    }

    public AudioPlayerBot(int id, string name, AudioPlayerBase audioPlayerBase, Player player)
    {
        Player = player;
        AudioPlayerBase = audioPlayerBase;
        ID = id;
        Name = name;
    }
    public int ID { get; private set; }
    public string Name { get; private set; }
    public Player Player { get; private set; }
    public AudioPlayerBase AudioPlayerBase { get; private set; }

    public VoiceChatChannel VoiceChatChannel { get => AudioPlayerBase.BroadcastChannel; set => AudioPlayerBase.BroadcastChannel = value; }
    public float Volume { get => AudioPlayerBase.Volume; set => AudioPlayerBase.Volume = value; }
    public bool Loop { get => AudioPlayerBase.Loop; set => AudioPlayerBase.Loop = value; }
    public bool Continue { get => AudioPlayerBase.Continue; set => AudioPlayerBase.Continue = value; }
    public bool LogDebug { get => AudioPlayerBase.LogDebug; set => AudioPlayerBase.LogDebug = value; }
    public bool Shuffle { get => AudioPlayerBase.Shuffle; set => AudioPlayerBase.Shuffle = value; }

    public void AddAudioEnqueue(string audio, int pos) => AudioPlayerBase.Enqueue(audio, pos);

    public void StopPlayerFromPlaying(List<int> players)
    {
        foreach (var player in players)
        {
            AudioPlayerBase.BroadcastTo.Remove(player);
        }
    }

    public void PlayAudioFromFile(string path, bool loop = false, float volume = 100, VoiceChatChannel channel = VoiceChatChannel.Intercom, bool shuffle = false, bool logdebug = false, bool @continue = true)
    {
        VoiceChatChannel = channel;
        Volume = volume;
        Loop = loop;
        Shuffle = shuffle;
        Continue = @continue;
        LogDebug = logdebug;
        AudioPlayerBase.Enqueue(Extensions.PathCheck(path), -1);
        AudioPlayerBase.Play(0);

    }

    public void PlayFromFilePlayer(List<int> players, string path, bool loop = false, float volume = 100, VoiceChatChannel channel = VoiceChatChannel.Intercom, bool shuffle = false, bool logdebug = false, bool @continue = true)
    {
        AudioPlayerBase.BroadcastTo = players;
        VoiceChatChannel = channel;
        Volume = volume;
        Loop = loop;
        Shuffle = shuffle;
        Continue = @continue;
        LogDebug = logdebug;
        AudioPlayerBase.Enqueue(Extensions.PathCheck(path), -1);
        AudioPlayerBase.Play(0);

    }

    public void StopAudio(bool clearAudioList = true) => AudioPlayerBase.Stoptrack(clearAudioList);

    public void Destroy()
    {
        if (Player is not Npc npc)
        {
            Log.Error($"A player with an id {Player.Id} not NPC");
            return;
        }
        npc.Destroy();
    }
}
