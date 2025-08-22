using HarmonyLib;
using System;
using UnityEngine;
namespace HitMarker.Patches.SpellPatches.Fireball
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
