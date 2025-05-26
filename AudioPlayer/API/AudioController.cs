using AudioPlayer.API.Container;
using AudioPlayer.Other;
using Exiled.API.Features;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;

namespace AudioPlayer.API;

public static class AudioController
{
    public static AudioPlayerBot SpawnDummy(int id = 99, string badgeText = "AudioPlayer BOT", string bagdeColor = "orange", string name = "Dedicated Server", RoleTypeId roleTypeId = RoleTypeId.None, bool ignored = true)
        => AudioPlayerBot.SpawnDummy(name, badgeText, bagdeColor, id, roleTypeId, ignored);

    public static void DisconnectDummy(int id = 99)
    {
        if (TryGetAudioPlayerContainer(id) is not AudioPlayerBot container)
        {
            return;
        }

        container.Destroy();
    }

    public static AudioPlayerBot TryGetAudioPlayerContainer(int id)
    {
        if (Plugin.AudioPlayerList.TryGetValue(id, out AudioPlayerBot container))
        {
            return container;
        }

        return null;
    }

    public static bool IsAudioPlayer(this int botId) => TryGetAudioPlayerContainer(botId) is not null;
    public static bool IsAudioPlayer(this Player player) => Plugin.AudioPlayerList.Values.Any(c => c.Player == player);

    public static IList<AudioPlayerBot> GetAllGetAudioPlayer() => Plugin.AudioPlayerList.Values.ToList();
    
    public static BotsList GetAudioPlayerInBotsList(int id) => Plugin.Instance.Config.BotsList.FirstOrDefault(x => x.BotId == id);
    public static BotsList GetAudioPlayerInBotsList(string name) => Plugin.Instance.Config.BotsList.FirstOrDefault(x => x.BotName == name);
}