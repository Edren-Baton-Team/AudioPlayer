using System.ComponentModel;

namespace AudioPlayer.Other;

public class BotsList
{
    public string BotName { get; set; } = "Dedicated Server";
    public int BotId { get; set; } = 99;

    [Description("Hide the AudioPlayer bot in the Player List?")]
    public bool ShowPlayerList { get; set; } = false;
    [Description("What will be written in Badge? (Only works if BadgeBots = true) | Set null to turn off")]
    public string BadgeText { get; set; } = "AudioPlayer BOT";
    [Description("What color will Badge? (Only works if BadgeBots = true)")]
    public string BadgeColor { get; set; } = "orange";
}
