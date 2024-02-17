using Exiled.API.Features;
using Exiled.API.Features.Pools;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using static HarmonyLib.AccessTools;

namespace AudioPlayer;

/// <summary>
/// Patches <see cref="ServerConsole.RefreshOnlinePlayers"/> to prevent NPC's from appearing as real players in the server list count.
/// </summary>
// The code is officially taken from https://github.com/Exiled-Team/EXILED/pull/2313/
[HarmonyPatch(typeof(ServerConsole), nameof(ServerConsole.RefreshOnlinePlayers))]
internal static class NpcServerCountFix
{
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Pool.Get(instructions);

        int offset = 0;
        int index = newInstructions.FindIndex(i => i.opcode == OpCodes.Ldsflda) + offset;

        Label labelcontinue = (Label)newInstructions[index - 1].operand;

        newInstructions.InsertRange(index, new[]
        {
                new CodeInstruction(OpCodes.Ldloc_1),
                new(OpCodes.Call, Method(typeof(Player), nameof(Player.Get), new[] { typeof(ReferenceHub) })),
                new(OpCodes.Callvirt, PropertyGetter(typeof(Player), nameof(Player.IsNPC))),
                new(OpCodes.Brtrue_S, labelcontinue),
            });

        for (int z = 0; z < newInstructions.Count; z++)
            yield return newInstructions[z];

        ListPool<CodeInstruction>.Pool.Return(newInstructions);
    }
}