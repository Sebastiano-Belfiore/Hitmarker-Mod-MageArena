using FirstModProject.Patches.SpellPatches.FrostBolt;
using FirstModProject.Utils;
using FishNet.Object;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FirstModProject.Patches.SpellPatches
{
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
                FieldInfo ShotByAiField = typeof(FrostBoltController).GetField("ShotByAi", BindingFlags.NonPublic | BindingFlags.Instance);
                FieldInfo collidedField = typeof(FrostBoltController).GetField("collided", BindingFlags.NonPublic | BindingFlags.Instance);

                if (ShotByAiField == null || collidedField == null)
                {
                    LoggerUtils.LogWarning("FrostBoltControllerPatch", "Required private fields not found via reflection");
                    return;
                }
                bool ShotByAi = (bool)ShotByAiField.GetValue(__instance);
                if (ShotByAi)
                {
                    LoggerUtils.LogPatch("FrostBoltController", "Projectile shot by AI, skipping");
                    return;
                }

                bool collided = (bool)collidedField.GetValue(__instance);
                if (!collided)
                {
                    LoggerUtils.LogPatch("FrostBoltController", "Projectile hasn't collided yet, skipping");
                    return;
                }

                if (!patchHandler.IsLocalPlayerOwner(__instance.playerOwner)) return;

                patchHandler.ProcessHit(other.gameObject, __instance.playerOwner);
            }
            catch (Exception ex)
            {
                LoggerUtils.LogError("FrostBoltControllerPatch", $"Error in patch: {ex.Message}");
            }
        }


    }

    
}
