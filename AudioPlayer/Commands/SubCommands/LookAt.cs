using AudioPlayer.Other;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using PlayerRoles.FirstPersonControl;
using System;
using UnityEngine;

namespace AudioPlayer.Commands.SubCommands;

public class LookAt : ICommand, IUsageProvider
{
    public string Command => "lookat";

    public string[] Aliases { get; } = { "look" };

    public string Description => "AudioPlayer bot will be looking at you";

    public string[] Usage { get; } = { "Bot ID" };

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!sender.CheckPermission($"audioplayer.{Command}"))
        {
            response = $"You dont have perms to do that. Not enough perms: audioplayer.{Command}";
            return false;
        }
        Player player = Player.Get(sender);
        if (arguments.Count == 0)
        {
            response = "Usage: audio lookat {Bot ID}";
            return false;
        }
        if (!int.TryParse(arguments.At(0), out int id))
        {
            response = "Specify a number, other characters are not accepted";
            return true;
        }

        if (Extensions.TryGetAudioBot(id, out FakeConnectionList fakeConnection))
        {
            Player bot = Player.Get(fakeConnection.hubPlayer);
            Vector3 direction = player.Position - bot.Position;
            Quaternion quat = Quaternion.LookRotation(direction, Vector3.up);
            FpcMouseLook mouseLook = ((IFpcRole)bot.ReferenceHub.roleManager.CurrentRole).FpcModule.MouseLook;
            (ushort horizontal, ushort vertical) = Extensions.ToClientUShorts(quat);
            mouseLook.ApplySyncValues(horizontal, vertical);
            response = $"Rotated {bot.Nickname} to the target {player.Nickname}";
            return true;
        }
        else
        {
            response = $"Bot with the ID {id} was not found.";
            return false;
        }
    }
}