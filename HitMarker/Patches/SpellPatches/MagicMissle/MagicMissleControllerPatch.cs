
using HarmonyLib;
using HitMarker.Utils;
using System;
using System.Reflection;
using UnityEngine;

namespace HitMarker.Patches.SpellPatches.MagicMissle
{
    [HarmonyPatch]
    public static class MagicMissle_Patches
    {

        private static MagicMissileHandler patchHandler = new MagicMissileHandler();

        [HarmonyPatch(typeof(MagicMissleController), "OnCollisionEnter")]
        [HarmonyPostfix]
        static void OnCollisionEnter_Postfix(MagicMissleController __instance, Collision other)
        {
            try
            {
                // Get private fields via reflection
                FieldInfo shotByAiField = typeof(MagicMissleController).GetField("shotByAi", BindingFlags.NonPublic | BindingFlags.Instance);
                FieldInfo collidedField = typeof(MagicMissleController).GetField("collided", BindingFlags.NonPublic | BindingFlags.Instance);

                if (shotByAiField == null || collidedField == null)
                {
                    LoggerUtils.LogError("MagicMissile", "Required fields not found via reflection");
                    return;
                }

                bool shotByAi = (bool)shotByAiField.GetValue(__instance);
                bool collided = (bool)collidedField.GetValue(__instance);

                if (shotByAi || !collided) return;

                patchHandler.ProcessHit(other.gameObject, __instance.playerOwner);
            }
            catch (Exception ex)
            {
                patchHandler.LogError($"Patch error: {ex.Message}");
            }
        }
    }
}
