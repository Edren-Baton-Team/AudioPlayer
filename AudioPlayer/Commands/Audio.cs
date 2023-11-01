using AudioPlayer.Commands.SubCommands;
using CommandSystem;
using Exiled.Permissions.Extensions;
using System;
using static AudioPlayer.Plugin;

namespace AudioPlayer.Commands;
[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class AudioCommand : ParentCommand
{
    public AudioCommand() => LoadGeneratedCommands();
    public override string Command => "audiocommand";

    public override string[] Aliases => plugin.Config.CommandName == null ? new string[] { "audio", "au" } : plugin.Config.CommandName;

    public override string Description => "Audio Command";

    public override void LoadGeneratedCommands()
    {
        RegisterCommand(new Add());
        RegisterCommand(new Enqueue());
        RegisterCommand(new Kick());
        RegisterCommand(new LookAt());
        RegisterCommand(new Loop());
        RegisterCommand(new NickName());
        RegisterCommand(new PFP());
        RegisterCommand(new Play());
        RegisterCommand(new SPFP());
        RegisterCommand(new Stop());
        RegisterCommand(new VoiceChannel());
        RegisterCommand(new Volume());
    }

    protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        response = "\nPlease enter a valid subcommand:";

        foreach (ICommand command in AllCommands)
        {
            if (sender.CheckPermission($"audioplayer.{command.Command}"))
            {
                response += $"\n\n<color=yellow><b>- {command.Command} ({string.Join(", ", command.Aliases)})\naudiocommand {command.Command} {"{" + string.Join("} {", (command as IUsageProvider).Usage) + "}"}</b></color>\n<color=white>{command.Description}</color>";
            }
        }
        return false;
    }
}