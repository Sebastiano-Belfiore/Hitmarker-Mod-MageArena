using HitMarkerMod.Utils;
using FishNet.Object;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HitMarkerMod.Patches.SpellPatches.DarkBlast
{
    [HarmonyPatch]
    public static class DarkBlast_Patches
    {

        private static GameObject owner;
        private static DarkBlastHandler patchHandler = new DarkBlastHandler();

        [HarmonyPatch(typeof(DarkBlastController), "CastDarkBlast")]
        [HarmonyPostfix]
        public static void CastDarkBlast_Postfix(DarkBlastController __instance, Vector3 fwdVector, GameObject ownerob)
        {
            try
            {
                owner = ownerob;
                patchHandler.LogDebug("Owner set");
                __instance.StartCoroutine(CastDarkBlast_HitDetectionRoutine(__instance));
            }
            catch (Exception ex)
            {
                patchHandler.LogError($"Patch error: {ex.Message}");
            }
        }


        private static IEnumerator CastDarkBlast_HitDetectionRoutine(DarkBlastController instance)
        {
           
            yield return new WaitForSeconds(0.25f);

            
            float num = 70f;
            Vector3 boxHalfExtents = new Vector3(0.9f, 0.9f, num / 2f);
            Vector3 boxCenter = instance.transform.position + instance.transform.forward.normalized * (num / 2f);
            Quaternion lookrot = Quaternion.LookRotation(instance.transform.forward, instance.transform.up);

            
            for (float timer = 0f; timer <= 0.8f; timer += 0.1f)
            {

             
                foreach (Collider collider in Physics.OverlapBox(boxCenter, boxHalfExtents, lookrot, instance.playerLayerMask))
                {
                    patchHandler.ProcessHit(collider.gameObject, owner);
                }
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
