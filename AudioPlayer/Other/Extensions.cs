﻿using System.IO;
using System.Linq;

namespace AudioPlayer.Other;
public static class Extensions
{
    //Creates a directory if it does not exist.
    public static void CreateDirectory()
    {
        if (!Directory.Exists(Plugin.plugin.AudioPath))
            Directory.CreateDirectory(Plugin.plugin.AudioPath);
    }
    public static bool IsAudioPlayerBot(this ReferenceHub player) => Plugin.plugin.FakeConnectionsIds.Values.Any(x => x.hubPlayer == player);
}