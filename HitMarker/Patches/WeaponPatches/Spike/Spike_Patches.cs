using HarmonyLib;
using HitMarker.Utils;
using System;
using UnityEngine;

namespace HitMarker.Patches.WeaponPatches.Spike
{
    [HarmonyPatch]
    public static class Spike_Patches
    {
        [HarmonyPatch(typeof(SpikedRootController), "Interaction")]
        [HarmonyPrefix]
        public static void SpikedRootController_OnInteraction_Prefix(SpikedRootController __instance, GameObject player)
        {
            LoggerUtils.LogPatch("Spike", $"SpikedRootController.Interaction called by player: {player.name}. Starting to process spikes.");

            try
            {
                if (__instance == null || player == null)
                {
                    LoggerUtils.LogError("Spike", "Null reference in Interaction prefix. Aborting patch.");
                    return;
                }

                foreach (GameObject spikePrefab in __instance.spikes)
                {
                    if (spikePrefab == null)
                    {
                        LoggerUtils.LogWarning("Spike", "Skipping null spikePrefab in array.");
                        continue;
                    }

                    LoggerUtils.LogDebug("Spike", $"Processing spikePrefab: {spikePrefab.name}.");

                    if (spikePrefab.GetComponent<SpikesCollision>() == null)
                    {
                        var collider = spikePrefab.GetComponent<Collider>();
                        if (collider == null)
                        {
                            collider = spikePrefab.AddComponent<BoxCollider>();
                        }
                        collider.isTrigger = true;

                        var spikeCollision = spikePrefab.AddComponent<SpikesCollision>();


                    }
                    else
                    {
                        LoggerUtils.LogDebug("Spike", $"SpikesCollision already exists on {spikePrefab.name}. Skipping.");
                    }
                    spikePrefab.GetComponent<SpikesCollision>().owner = player;
                }
                LoggerUtils.LogPatch("Spike", "SpikedRootController.Interaction prefix finished successfully.");
            }
            catch (Exception ex)
            {
                LoggerUtils.LogError("Spike", $"Exception in Interaction patch prefix: {ex.Message}");
                // Puoi anche loggare lo stack trace per più dettagli
                // LoggerUtils.LogCriticalError("Spike", "Exception in Interaction patch prefix", ex);
            }
        }
    }
}

