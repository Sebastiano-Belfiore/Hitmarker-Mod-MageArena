using HarmonyLib;
using HitMarker.Utils;
using System;
using UnityEngine;

namespace HitMarker.Patches.SpellPatches.Wisp
{

    public static class Wisp_Patches
    {

        // Patch sul metodo SummonIceBox - quando level == 0 è un wisp hit
        [HarmonyPatch(typeof(PlayerMovement), "SummonIceBox")]
        [HarmonyPrefix]
        static void SummonIceBox_Prefix(PlayerMovement __instance, int lvl, GameObject playerownner)
        {
            try
            {
                // Se level != 0, non è un wisp, quindi ignora
                if (lvl != 0)
                {

                    LoggerUtils.LogDebug("SummonIceBox", $" Level {lvl} detected - not a wisp hit. Skipping.");
                    return;
                }

                if (__instance == null)
                {
                    LoggerUtils.LogError("SummonIceBox", " __instance or ownerobj is NULL. Aborting.");
                    return;
                }
                if (playerownner == null)
                {
                    LoggerUtils.LogDebug("SummonIceBox", " Owner object is NULL. HitByAI.");
                }

                // Verifica che sia il proprietario locale
                if (!NetworkUtils.IsLocalPlayerOwner(playerownner))
                {
                    LoggerUtils.LogDebug("SummonIceBox", "Owner is not the local player. Skipping hit marker.");
                    return;
                }

                LoggerUtils.LogDebug("SummonIceBox", $"Wisp hit detected (level 0)! Target: {__instance.name}, Owner: {playerownner.name}");

                // Mostra l'hit marker per il wisp
                Mod.Instance?.ShowHitmarkerInstance();
                LoggerUtils.LogHitDetection("SummonIceBox", ColliderUtils.GetTargetInfo(__instance.gameObject), true);
            }
            catch (Exception ex)
            {
                LoggerUtils.LogError("SummonIceBox", $"CRITICAL ERROR: {ex.Message}");
            }
        }
    }
}
