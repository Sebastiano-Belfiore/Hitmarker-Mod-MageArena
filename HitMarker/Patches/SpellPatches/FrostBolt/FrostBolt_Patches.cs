using HarmonyLib;
using HitMarker.Utils;
using System;
using System.Reflection;
using UnityEngine;

namespace HitMarker.Patches.SpellPatches.FrostBolt
{
    [HarmonyPatch]
    public static class FrostBolt_Patches
    {
        private static FrostBoltHandler patchHandler = new FrostBoltHandler();

        [HarmonyPatch(typeof(FrostBoltController), "OnTriggerEnter")]
        [HarmonyPostfix]
        static void OnTriggerEnter_Postfix(FrostBoltController __instance, Collider other)
        {
            patchHandler.LogPatch("FrostBolt trigger detected");
            try
            {

                FieldInfo shotByAiField = typeof(FrostBoltController).GetField("ShotByAi", BindingFlags.NonPublic | BindingFlags.Instance);
                FieldInfo collidedField = typeof(FrostBoltController).GetField("collided", BindingFlags.NonPublic | BindingFlags.Instance);

                if (shotByAiField == null || collidedField == null)
                {
                    LoggerUtils.LogError("FrostBolt", "Required fields not found via reflection");
                    return;
                }

                bool shotByAi = (bool)shotByAiField.GetValue(__instance);
                bool collided = (bool)collidedField.GetValue(__instance);

                if (shotByAi || !collided) return;
                if (!patchHandler.IsLocalPlayerOwner(__instance.playerOwner)) return;

                patchHandler.ProcessHit(other.gameObject, __instance.playerOwner);
            }
            catch (Exception ex)
            {
                patchHandler.LogError($"Patch error: {ex.Message}");
            }
        }


    }


}
