using Exiled.API.Enums;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using static AudioPlayer.Plugin;

namespace AudioPlayer.Other;

public class BotsList
{
    public string BotName { get; set; }
    public int BotId { get; set; }

    [Description("Hide the AudioPlayer bot in the Player List?")]
    public bool ShowPlayerList { get; set; } = false; 
    [Description("What will be written in Badge? (Only works if BadgeBots = true) | Set null to turn off")]
    public string BadgeText { get; set; } = "AudioPlayer BOT";
    [Description("What color will Badge? (Only works if BadgeBots = true)")]
    public string BadgeColor { get; set; } = "orange";
    public static BotsList Get(int id) => plugin.Config.BotsList.FirstOrDefault(x => x.BotId == id);
    public static BotsList Get(string name) => plugin.Config.BotsList.FirstOrDefault(x => x.BotName == name);
}
