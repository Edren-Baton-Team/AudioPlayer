using AudioPlayer.Commands.SubCommands;
using CommandSystem;
using Exiled.Permissions.Extensions;
using System;
using System.Linq;
using static AudioPlayer.Plugin;

namespace AudioPlayer.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class AudioCommand : ParentCommand
{
    public AudioCommand() => LoadGeneratedCommands();
    public override string Command => "audiocommand";

    public override string[] Aliases => Instance.Config.CommandName ?? ["audio", "au"];

    public override string Description => "Audio Command";

    public sealed override void LoadGeneratedCommands()
    {
        RegisterCommand(new Add());
        RegisterCommand(new Enqueue());
        RegisterCommand(new Kick());
        RegisterCommand(new Loop());
        RegisterCommand(new Nickname());
        RegisterCommand(new PFP());
        RegisterCommand(new Play());
        RegisterCommand(new SPFP());
        RegisterCommand(new Stop());
        RegisterCommand(new VoiceChannel());
        RegisterCommand(new Volume());
    }

    public override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        response = AllCommands.Where(command => sender.CheckPermission($"audioplayer.{command.Command}")).Aggregate("\nPlease enter a valid subcommand:", (current, command) => current + $"\n\n<color=yellow><b>- {command.Command} ({string.Join(", ", command.Aliases)})\naudiocommand {command.Command} {"{" + string.Join("} {", (command as IUsageProvider)!.Usage) + "}"}</b></color>\n<color=white>{command.Description}</color>");

        return false;
    }
}