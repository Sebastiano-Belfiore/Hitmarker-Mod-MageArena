using FirstModProject.Utils;
using FishNet.Object;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FirstModProject.Patches.WeaponPatches
{

    public static class Torch_Patches
    {
        private static TorchHandler patchHandler = new TorchHandler();
        public static GameObject owner;


        [HarmonyPatch(typeof(TorchController), "Interaction")]
        [HarmonyPrefix]
        public static void OnInteraction_Prefix(TorchController __instance, GameObject player)
        {
            patchHandler.LogPatch("TorchController trigger detected");
            owner = player;
        }

        [HarmonyPatch(typeof(TorchHitDetection), "OnTriggerEnter")]
        [HarmonyPostfix]
        public static void OnTriggerEnter_Postfix(TorchHitDetection __instance, Collider other)
        {
            patchHandler.LogPatch("TorchHitDetection trigger detected");

            try
            {
           
                if (!Input.GetKey(KeyCode.Mouse0))
                {
                   // patchHandler.LogPatch("GetKey(KeyCode.Mouse0) not triggered");
                    return;
                }
                //patchHandler.LogPatch("GetKey(KeyCode.Mouse0) triggered");

                patchHandler.ProcessHit(__instance.toerch.HitSubject, owner);

            }
            catch (Exception ex)
            {
                patchHandler.LogError($"Error in patch: {ex.Message}");
            }
        }
    }
}
