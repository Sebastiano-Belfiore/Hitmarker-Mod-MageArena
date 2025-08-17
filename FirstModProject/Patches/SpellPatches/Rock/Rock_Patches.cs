using HitMarkerMod.Utils;
using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class Rock_Patches
{
    private static GameObject owner;
    private static RockHandler patchHandler = new RockHandler();
    private static readonly HashSet<int> processedInstanceIDs = new HashSet<int>();

    [HarmonyPatch(typeof(RockSpellController), "StartRockRoutine")]
    [HarmonyPostfix]
    static void StartRockRoutine_Postfix(RockSpellController __instance, GameObject playerOwner)
    {
        owner = playerOwner;
        patchHandler.LogDebug("Owner set");
    }

    [HarmonyPatch(typeof(RockCheckSphere), "FixedUpdate")]
    [HarmonyPostfix]
    static void FixedUpdate_Postfix(RockCheckSphere __instance)
    {
        int instanceId = __instance.GetInstanceID();
        if (processedInstanceIDs.Contains(instanceId)) return;

        try
        {
            if (!patchHandler.IsLocalPlayerOwner(owner)) return;

            Collider[] colliders = Physics.OverlapSphere(__instance.transform.position, __instance.radius, __instance.playerLayer);

            foreach (Collider collider in colliders)
            {
                if (patchHandler.ProcessHit(collider.gameObject, owner))
                {
                    processedInstanceIDs.Add(instanceId);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            patchHandler.LogError($"Patch error: {ex.Message}");
        }
    }
}