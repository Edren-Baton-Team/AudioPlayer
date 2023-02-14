using Exiled.API.Interfaces;
using System.ComponentModel;

namespace AudioPlayer
{
    public class Config : IConfig
    {
        [Description("Plugin Enabled?")]
        public bool IsEnabled { get; set; } = true;

        [Description("Enable developer mode?")]
        public bool Debug { get; set; } = true;

        [Description("The name of the bot when the file will sound")]
        public string BotName { get; set; } = "Dedicated Server";
    }
}
