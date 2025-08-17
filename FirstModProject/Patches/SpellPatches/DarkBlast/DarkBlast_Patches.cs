using FirstModProject.Patches.WeaponPatches;
using FishNet.Object;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FirstModProject.Patches.SpellPatches.DarkBlast
{
    public static class DarkBlast_Patches
    {

        private static GameObject owner;
        private static DarkBlastHandler patchHandler = new DarkBlastHandler();
        [HarmonyPatch("CastDarkBlast")]
        [HarmonyPostfix]
        public static void CastDarkBlast_Postfix(DarkBlastController __instance, Vector3 fwdVector, GameObject ownerob)
        {

            owner = ownerob;
            patchHandler.LogPatch("interaction detected owner setted");
            __instance.StartCoroutine(CastDarkBlast_HitDetectionRoutine(__instance));
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
