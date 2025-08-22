using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;



namespace HitMarker.Patches.SpellPatches.Rock
{

    [HarmonyPatch]
    public static class Rock_Patches
    {
        private static RockHandler patchHandler = new RockHandler();
        private static readonly Queue<int> processedInstanceQueue = new Queue<int>();
        private static readonly HashSet<int> processedInstanceIDs = new HashSet<int>();
        private const int MAX_PROCESSED_INSTANCES = 100;

        [HarmonyPatch(typeof(RockSpellController), "StartRockRoutine")]
        [HarmonyPostfix]
        static void StartRockRoutine_Postfix(RockSpellController __instance, GameObject playerOwner)
        {
            patchHandler.LogDebug($"[StartRockRoutine] Patch started. Instance: {__instance?.name ?? "NULL"}, Owner: {playerOwner?.name ?? "NULL"}.");

            try
            {
                if (__instance == null || playerOwner == null)
                {
                    patchHandler.LogError("[StartRockRoutine] __instance or playerOwner is NULL. Aborting.");
                    return;
                }

                patchHandler.LogDebug($"[StartRockRoutine] Attempting to set owner on grpo for instance {__instance.GetInstanceID()}.");

                if (__instance.grpo == null)
                {
                    patchHandler.LogError($"[StartRockRoutine] __instance.grpo is NULL. Cannot assign owner.");
                    return;
                }

                __instance.grpo.owner = playerOwner;
                patchHandler.LogDebug($"[StartRockRoutine] Owner successfully assigned. Current owner: {__instance.grpo.owner?.name ?? "NULL"}.");
            }
            catch (Exception ex)
            {
                patchHandler.LogError($"[StartRockRoutine] CRITICAL ERROR: {ex.Message}");
            }
        }

        [HarmonyPatch(typeof(RockCheckSphere), "FixedUpdate")]
        [HarmonyPostfix]
        static void FixedUpdate_Postfix(RockCheckSphere __instance)
        {
            try
            {
                if (__instance == null)
                {
                    patchHandler.LogError("[FixedUpdate] __instance is NULL. Aborting.");
                    return;
                }

                int instanceId = __instance.GetInstanceID();

                // Se già processato, non fare nulla - l'oggetto verrà distrutto comunque
                if (processedInstanceIDs.Contains(instanceId))
                {
                    // Non logghiamo più per evitare spam nei log
                    return;
                }

                if (__instance.grpo == null)
                {
                    patchHandler.LogError($"[FixedUpdate] grpo is NULL for instance {instanceId}. Cannot check owner.");
                    return;
                }

                GameObject owner = __instance.grpo.owner;
                patchHandler.LogDebug($"[FixedUpdate] Owner found for instance {instanceId}: {owner?.name ?? "NULL"}.");

                if (owner == null || !patchHandler.IsLocalPlayerOwner(owner))
                {
                    patchHandler.LogDebug($"[FixedUpdate] Owner is not the local player or is NULL. Skipping hit check.");
                    return;
                }

                patchHandler.LogDebug($"[FixedUpdate] Owner is local player. Performing OverlapSphere check at position {__instance.transform.position}.");

                Collider[] colliders = Physics.OverlapSphere(__instance.transform.position, __instance.radius, __instance.playerLayer);
                patchHandler.LogDebug($"[FixedUpdate] Found {colliders.Length} colliders.");

                foreach (Collider collider in colliders)
                {
                    if (collider.gameObject == null)
                    {
                        patchHandler.LogError("[FixedUpdate] Skipping NULL collider in list.");
                        continue;
                    }

                    patchHandler.LogDebug($"[FixedUpdate] Checking collider: {collider.gameObject.name}.");

                    if (patchHandler.ProcessHit(collider.gameObject, owner))
                    {
                        patchHandler.LogDebug($"[FixedUpdate] Hit processed successfully for instance {instanceId} with {collider.gameObject.name}.");

                        // Aggiungi alla coda e al set
                        processedInstanceQueue.Enqueue(instanceId);
                        processedInstanceIDs.Add(instanceId);

                        // Se la coda è troppo grande, rimuovi il più vecchio
                        if (processedInstanceQueue.Count > MAX_PROCESSED_INSTANCES)
                        {
                            int oldestId = processedInstanceQueue.Dequeue();
                            processedInstanceIDs.Remove(oldestId);
                            patchHandler.LogDebug($"[FixedUpdate] Removed oldest processed instance {oldestId} to maintain size limit.");
                        }

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                patchHandler.LogError($"[FixedUpdate] CRITICAL ERROR: {ex.Message}");
            }
        }
    }
}