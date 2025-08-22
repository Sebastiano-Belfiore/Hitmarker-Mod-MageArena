using HarmonyLib;
using System;
using UnityEngine;


namespace HitMarker.Patches.WeaponPatches.Torch
{
    [HarmonyPatch]
    public static class Torch_Patches
    {
        private static TorchHandler patchHandler = new TorchHandler();
        //  private static GameObject owner;

        /* [HarmonyPatch(typeof(TorchController), "Interaction")]
         [HarmonyPrefix]
         public static void OnInteraction_Prefix(TorchController __instance, GameObject player)
         {
             owner = player;
             patchHandler.LogDebug("Owner set");
         } */
        [HarmonyPatch(typeof(TorchHitDetection), "OnTriggerEnter")]
        [HarmonyPostfix]
        public static void OnTriggerEnter_Postfix(TorchHitDetection __instance, Collider other)
        {
            try
            {
                if (!Input.GetKey(KeyCode.Mouse0)) return;

                patchHandler.ProcessHit(__instance.toerch.HitSubject, null);
            }
            catch (Exception ex)
            {
                patchHandler.LogError($"Patch error: {ex.Message}");
            }
        }
    }
}

