using HarmonyLib;
using RoundRestarting;
using AudioPlayer.API;

namespace AudioPlayer;

[HarmonyPatch(typeof(RoundRestart), nameof(RoundRestart.InitiateRoundRestart))] // Отключает Hitmarker
static class HitmarkerPatch
{
    public static void Prefix()
    {
        foreach(var audioBot in Plugin.FakeConnectionsIds.Keys)
        {
            AudioController.DisconnectDummy(audioBot);
        }
    }
}