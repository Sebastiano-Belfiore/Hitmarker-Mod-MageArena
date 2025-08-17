using FirstModProject.Utils;
using FishNet.Object;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FirstModProject.Patches.WeaponPatches
{
    public static class Swords_Patches
    {
        public static GameObject owner;
        private static MushroomSwordHandler patchHandler = new MushroomSwordHandler();

        public static readonly HashSet<int> processedInstanceIDs = new HashSet<int>();

        [HarmonyPatch(typeof(MushroomSword), "Interaction")]
        [HarmonyPrefix]
        public static void OnInteraction_Prefix(MushroomSword __instance, GameObject player)
        {
            
            owner = player;
            patchHandler.LogPatch("interaction detected owner setted");
        }

        [HarmonyPatch(typeof(WeaponHitDetection), "OnTriggerEnter")]
        [HarmonyPostfix]
        public static void OnTriggerEnter_Postfix(WeaponHitDetection __instance, Collider other)
        {
            patchHandler.LogPatch("trigger detected");
            try
            {

                if (!Input.GetKey(KeyCode.Mouse0))
                {
                    // patchHandler.LogPatch("GetKey(KeyCode.Mouse0) not triggered");
                    return;
                }
                //patchHandler.LogPatch("GetKey(KeyCode.Mouse0) triggered");
                
                if(__instance.swerd && __instance.swerd is MushroomSword)
                {
                    patchHandler.ProcessHit(__instance.swerd.HitSubject, owner);
                }
            }
            catch (Exception ex)
            {
                patchHandler.LogError($"Error in patch: {ex.Message}");
            }
        }
    }
}

