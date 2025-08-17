using FirstModProject.Patches.SpellPatches.LightningBolt;
using FirstModProject.Patches.SpellPatches.MagicMissle;
using FirstModProject.Utils;
using FishNet.Object;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FirstModProject.Patches.WeaponPatches
{
    public static class MagicMissle_Patches {

        private static MagiMissleHandler patchHandler = new MagiMissleHandler();


        [HarmonyPatch(typeof(MagicMissleController), "OnCollisionEnter")]
        [HarmonyPostfix]
        static void OnCollisionEnter_Postfix(MagicMissleController __instance, Collision other)
        {
            patchHandler.LogPatch("checking for hits");
            try
            {
                /*  // Aggiungi un controllo per 'other' e 'other.collider'
                  if (other == null || other.collider == null)
                  {
                      Mod.Log.LogWarning("[MagicMissleControllerOnCollisionEnterrPatch] Collisione o collider nullo. Interrompo l'esecuzione.");
                      return;
                  } */

                // Otteniamo i campi privati 'shotByAi' e 'collided' tramite reflection
                FieldInfo shotByAiField = typeof(MagicMissleController).GetField("shotByAi", BindingFlags.NonPublic | BindingFlags.Instance);
                FieldInfo collidedField = typeof(MagicMissleController).GetField("collided", BindingFlags.NonPublic | BindingFlags.Instance);

                if (shotByAiField == null || collidedField == null)
                {
                    patchHandler.LogPatch("Required private fields not found via reflection");
                    return;
                }

                bool shotByAi = (bool)shotByAiField.GetValue(__instance);

                if (shotByAi)
                {
                    patchHandler.LogPatch("Projectile shot by AI, skipping");
                    return;
                }

                bool collided = (bool)collidedField.GetValue(__instance);
                // Controlla se il proiettile è già colliso, altrimenti usciamo.
                if (!collided)
                {
                    patchHandler.LogPatch("Projectile hasn't collided yet, skipping");
                    return;
                }

                patchHandler.ProcessHit(other.gameObject, __instance.playerOwner);
            }
            catch (Exception ex)
            {
                patchHandler.LogError($"Error in patch: {ex.Message}");
            }
        }
    }
}
