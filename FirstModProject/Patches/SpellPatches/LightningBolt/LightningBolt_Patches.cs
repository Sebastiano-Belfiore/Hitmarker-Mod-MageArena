
using HitMarkerMod.Utils;
using FishNet.Object;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HitMarkerMod.Patches.SpellPatches.LightningBolt
{
    [HarmonyPatch]
    public static class LightningBolt_Patches
    {
        private static LightningHandler patchHandler = new LightningHandler();

        [HarmonyPatch(typeof(LightningBoltDamage), "DoDmg")]
        [HarmonyPostfix]
        public static void DoDmg_Postfix(LightningBoltDamage __instance, GameObject OwnerObj)
        {
            patchHandler.LogPatch("LightningBolt detected, checking for hits");
            try
            {
                if (!patchHandler.IsLocalPlayerOwner(OwnerObj)) return;

               
                FieldInfo heightField = typeof(LightningBoltDamage).GetField("height", BindingFlags.NonPublic | BindingFlags.Instance);
                FieldInfo widthField = typeof(LightningBoltDamage).GetField("width", BindingFlags.NonPublic | BindingFlags.Instance);

                if (heightField == null || widthField == null)
                {
                    patchHandler.LogError("Required fields not found via reflection");
                    return;
                }

                float height = (float)heightField.GetValue(__instance);
                float width = (float)widthField.GetValue(__instance);

                Vector3 center = __instance.transform.position + Vector3.up * (height / 2f);
                Vector3 halfExtents = new Vector3(width / 2f, height / 2f, width / 2f);

                Collider[] colliders = Physics.OverlapBox(center, halfExtents, Quaternion.identity, __instance.lrm);

                foreach (Collider collider in colliders)
                {
                    patchHandler.ProcessHit(collider.gameObject, OwnerObj);
                }
            }
            catch (Exception ex)
            {
                patchHandler.LogError($"Patch error: {ex.Message}");
            }
        }
    }
}
