using HitMarkerMod.Utils;
using HarmonyLib;
using System;
using UnityEngine;

public static class Swords_Patches
{
    private static GameObject owner;
    private static MushroomSwordHandler patchHandler = new MushroomSwordHandler();

    [HarmonyPatch(typeof(MushroomSword), "Interaction")]
    [HarmonyPrefix]
    public static void OnInteraction_Prefix(MushroomSword __instance, GameObject player)
    {
        owner = player;
        patchHandler.LogDebug("Owner set");
    }

    [HarmonyPatch(typeof(WeaponHitDetection), "OnTriggerEnter")]
    [HarmonyPostfix]
    public static void OnTriggerEnter_Postfix(WeaponHitDetection __instance, Collider other)
    {
        try
        {
            if (!Input.GetKey(KeyCode.Mouse0)) return;

            if (__instance.swerd != null && __instance.swerd is MushroomSword)
            {
                patchHandler.ProcessHit(__instance.swerd.HitSubject, owner);
            }
        }
        catch (Exception ex)
        {
            patchHandler.LogError($"Patch error: {ex.Message}");
        }
    }
}