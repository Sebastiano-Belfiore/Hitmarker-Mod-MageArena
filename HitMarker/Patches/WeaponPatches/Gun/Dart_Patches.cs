using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace HitMarker.Patches.WeaponPatches.Gun
{
    [HarmonyPatch]
    public static class Dart_Patches
    {
        private static DartGunHandler patchHandler = new DartGunHandler();

        // Dizionario per tracciare l'ultimo giocatore che ha usato ogni DartGunController
        private static readonly Dictionary<int, GameObject> dartGunLastUsers = new Dictionary<int, GameObject>();

        // Dizionario per tracciare se un proiettile ha già generato un hitmarker
        private static readonly Dictionary<int, bool> processedDartHits = new Dictionary<int, bool>();

        // Patch su Interaction per tracciare chi ha sparato
        [HarmonyPatch(typeof(DartGunController), "Interaction")]
        [HarmonyPrefix]
        static void Interaction_Prefix(DartGunController __instance, GameObject player)
        {
            try
            {
                if (__instance == null || player == null)
                    return;

                int instanceId = __instance.GetInstanceID();
                dartGunLastUsers[instanceId] = player;
                patchHandler.LogDebug($"[Interaction] Player {player.name} is using dart gun {instanceId}.");
            }
            catch (Exception ex)
            {
                patchHandler.LogError($"[Interaction] CRITICAL ERROR: {ex.Message}");
            }
        }

        [HarmonyPatch(typeof(DartController), "OnTriggerEnter")]
        [HarmonyPrefix]
        static void OnTriggerEnter_Prefix(DartController __instance, Collider other)
        {
            try
            {
                // Se il proiettile ha già colpito qualcosa, non processare di nuovo
                if (processedDartHits.ContainsKey(__instance.GetInstanceID()))
                {
                    patchHandler.LogDebug("[OnTriggerEnter] Already processed collision for this dart. Skipping.");
                    return;
                }

                if (other.CompareTag("Ignorable") || other.CompareTag("hex"))
                {
                    patchHandler.LogDebug($"[OnTriggerEnter] Hit ignorable object: {other.name}. Skipping.");
                    return;
                }

                // Segna il proiettile come già processato
                processedDartHits[__instance.GetInstanceID()] = true;

                DartGunController dartGun = __instance.dgc;
                if (dartGun == null)
                {
                    patchHandler.LogDebug("[OnTriggerEnter] No DartGunController found. Skipping.");
                    return;
                }

                int instanceIdDartGun = dartGun.GetInstanceID();

                if (!dartGunLastUsers.ContainsKey(instanceIdDartGun))
                {
                    patchHandler.LogDebug("[OnTriggerEnter] No recorded user for this dart gun. Skipping.");
                    return;
                }
                GameObject owner = dartGunLastUsers[instanceIdDartGun];
                if (owner == null)
                {
                    patchHandler.LogError("[OnTriggerEnter] Recorded owner is NULL.");
                    return;
                }

                if (!patchHandler.IsLocalPlayerOwner(owner))
                {
                    patchHandler.LogDebug("[OnTriggerEnter] Owner is not the local player. Skipping hit marker.");
                    return;
                }

                patchHandler.ProcessHit(other.gameObject, owner);
            }
            catch (Exception ex)
            {
                patchHandler.LogError($"[OnTriggerEnter] CRITICAL ERROR: {ex.Message}");
            }
        }


        [HarmonyPatch(typeof(DartController), "HitSomething")]
        [HarmonyPostfix]
        static void HitSomething_Postfix(DartController __instance)
        {
            try
            {
                int instanceIdDartGun = __instance.dgc.GetInstanceID();
                int instanceIdDart = __instance.GetInstanceID();

                // Pulizia del dizionario di tracciamento della collisione
                if (processedDartHits.ContainsKey(instanceIdDart))
                {
                    processedDartHits.Remove(instanceIdDart);
                    patchHandler.LogDebug($"[HitSomething_Postfix] Removed hit tracking for dart {instanceIdDart}.");
                }

                // Nota: non è necessario rimuovere l'entry da dartGunLastUsers qui.
                // Quella voce deve rimanere finché la DartGunController è attiva.
            }
            catch (Exception ex)
            {
                patchHandler.LogError($"[HitSomething_Postfix] CRITICAL ERROR: {ex.Message}");
            }
        }
    }
}