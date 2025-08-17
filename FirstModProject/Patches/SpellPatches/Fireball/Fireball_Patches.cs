
using HitMarkerMod.Utils;
using FishNet.Object;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HitMarkerMod.Patches.SpellPatches
{
    [HarmonyPatch]
    public static class Fireball_Patches
    {
        private static FireballHandler patchHandler = new FireballHandler();
        
        [HarmonyPatch(typeof(ExplosionController), "Explode")]
        [HarmonyPostfix]
        static void Explode_Postfix(ExplosionController __instance, GameObject owner)
        {
            try
            {
                if (!patchHandler.IsLocalPlayerOwner(owner)) return;

                float sphereRadius = 8f + (float)__instance.level * 0.4f;
                Collider[] colliders = Physics.OverlapSphere(__instance.transform.position, sphereRadius, __instance.PlayerLayer);

                foreach (Collider collider in colliders)
                {
                    patchHandler.ProcessHit(collider.gameObject, owner);
                }
            }
            catch (Exception ex)
            {
                patchHandler.LogError($"Patch error: {ex.Message}");
            }
        }


    }
    

}
