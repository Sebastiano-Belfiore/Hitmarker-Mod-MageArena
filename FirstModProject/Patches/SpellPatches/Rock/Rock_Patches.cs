using FirstModProject.Utils;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FirstModProject.Patches.SpellPatches.Rock
{

    public static class Rock_Patches 
    {
        public static GameObject owner;
        public static RockHandler patchHandler = new RockHandler();
        public static readonly HashSet<int> processedInstanceIDs = new HashSet<int>();

        [HarmonyPatch(typeof(RockSpellController), "StartRockRoutine")]
        [HarmonyPostfix]
        static void StartRockRoutine_Postfix(RockSpellController __instance, GameObject playerOwner)
        {
            owner = playerOwner;
            patchHandler.LogPatch("owser setted");
            
            // Assegna il proprietario all'oggetto grpo non appena lo spell inizia
            /* if (__instance.grpo != null)
             {
                 __instance.grpo.owner = playerOwner;

                 LoggerUtils.LogPatch("RockSpellController", "Owner Setted");
             } */
        }

        [HarmonyPatch(typeof(RockCheckSphere), "FixedUpdate")]
        [HarmonyPostfix]
        static void FixedUpdate_Postfix(RockCheckSphere __instance)
        {
            patchHandler.LogPatch("Rock detected");
         
            if (processedInstanceIDs.Contains(__instance.GetInstanceID()))
            {
                patchHandler.LogPatch("This rock is just processed");
                return;
            }

            try
            {
                if (!patchHandler.IsLocalPlayerOwner(owner)) return;

                Collider[] colliders = Physics.OverlapSphere(__instance.transform.position, __instance.radius, __instance.playerLayer);
                foreach (Collider collider in colliders)
                {
                    if(patchHandler.ProcessHit(collider.gameObject, owner))
                    {
                        processedInstanceIDs.Add(__instance.GetInstanceID());
                    }
                }
            }
            catch (Exception ex)
            {
                patchHandler.LogError($"Error in patch: {ex.Message}");
            }
        }

    }

}
