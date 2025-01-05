using AudioPlayer.API.Container;
using AudioPlayer.Other;
using Exiled.API.Features;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;

namespace AudioPlayer.API;
public static class AudioController
{
    public static AudioPlayerBot SpawnDummy(int id = 99, string badgetext = "AudioPlayer BOT", string bagdecolor = "orange", string name = "Dedicated Server", RoleTypeId roleTypeId = RoleTypeId.None, bool ignored = true)
        => AudioPlayerBot.SpawnDummy(name, badgetext, bagdecolor, id, roleTypeId, ignored);
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
        if (Plugin.AudioPlayerList.TryGetValue(id, out var container))
        {
            return container;
        }

        return null;
    }
    public static bool IsAudioPlayer(this int botId) => TryGetAudioPlayerContainer(botId) is not null;
    public static bool IsAudioPlayer(this Player player) => GetAudioPlayer(player) is not null;

    public static IList<AudioPlayerBot> GetAllGetAudioPlayer() => Plugin.AudioPlayerList.Values.ToList();

    public static Player GetAudioPlayer(this Player player) => Plugin.AudioPlayerList.Values.FirstOrDefault(c => c.Player == player).Player;

    public static BotsList GetAudioPlayerInBotsList(int id) => Plugin.plugin.Config.BotsList.FirstOrDefault(x => x.BotId == id);
    public static BotsList GetAudioPlayerInBotsList(string name) => Plugin.plugin.Config.BotsList.FirstOrDefault(x => x.BotName == name);
}
