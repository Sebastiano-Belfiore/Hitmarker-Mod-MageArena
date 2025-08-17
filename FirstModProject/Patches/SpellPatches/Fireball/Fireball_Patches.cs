using FirstModProject.Patches.SpellPatches.Rock;
using FirstModProject.Patches.WeaponPatches;
using FirstModProject.Utils;
using FishNet.Object;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FirstModProject.Patches.SpellPatches
{
    public static class Fireball_Patches
    {
        private static FireballHandler patchHandler = new FireballHandler();
        
        [HarmonyPatch(typeof(ExplosionController), "Explode")]
        [HarmonyPostfix]
        static void Explode_Postfix(ExplosionController __instance, GameObject owner)
        {
            patchHandler.LogPatch(" checking for hits");
            try
            {
                if (!patchHandler.IsLocalPlayerOwner(owner))
                {
                    return;
                }
                float sphereRadius = 8f + (float)__instance.level * 0.4f;
                foreach (Collider collider in Physics.OverlapSphere(__instance.transform.position, sphereRadius, __instance.PlayerLayer))
                {
                    patchHandler.ProcessHit(collider.gameObject, owner);
                }
            }
            catch (Exception ex)
            {
                patchHandler.LogError($"Error in patch: {ex.Message}");
            }
        }


    }
    

}
