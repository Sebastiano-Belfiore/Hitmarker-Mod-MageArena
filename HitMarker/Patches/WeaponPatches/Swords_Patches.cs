using HarmonyLib;
using HitMarker.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace HitMarker.Patches.WeaponPatches
{
    [HarmonyPatch]
    public static class Swords_Patches
    {
        public static Dictionary<SwordController, GameObject> weaponOwners = new Dictionary<SwordController, GameObject>(); // Rendi public
        private static MushroomSwordHandler patchHandlerMushroomSword = new MushroomSwordHandler();
        private static FrogBladeHandler patchHandlerFrogBladeHandler = new FrogBladeHandler();
        private static ExcaliburSwordHandler patchHandlerExcaliburSword = new ExcaliburSwordHandler();
        private static FrogSpearHandler patchHandlerFrogSpearHandler = new FrogSpearHandler();
        private static void SetWeaponOwner(SwordController weapon, GameObject player, string weaponType)
        {
            weaponOwners[weapon] = player;

            // ✅ Aggiungi il componente tracker se non esiste
            var tracker = weapon.GetComponent<SwordOwnerTracker>();
            if (tracker == null)
            {
                tracker = weapon.gameObject.AddComponent<SwordOwnerTracker>();
                tracker.swordController = weapon;
            }
            tracker.owner = player;

            LoggerUtils.LogDebug("Swords_Patches", $"Owner set for {weaponType}: {player.name}");
        }



        [HarmonyPatch(typeof(MushroomSword), "Interaction")]
        [HarmonyPrefix]
        public static void MushroomSword_OnInteraction_Prefix(MushroomSword __instance, GameObject player)
        {
            SetWeaponOwner(__instance, player, "MushroomSword");
        }
        [HarmonyPatch(typeof(FrogBladeController), "Interaction")]
        [HarmonyPrefix]
        public static void FrogBladeController_OnInteraction_Prefix(FrogBladeController __instance, GameObject player)
        {
            SetWeaponOwner(__instance, player, "FrogBladeController");
        }
        [HarmonyPatch(typeof(ExcaliburController), "Interaction")]
        [HarmonyPrefix]
        public static void ExcaliburSword_OnInteraction_Prefix(ExcaliburController __instance, GameObject player)
        {
            SetWeaponOwner(__instance, player, " ExcaliburSword");
        }

        [HarmonyPatch(typeof(FrogSpear), "Interaction")]
        [HarmonyPrefix]
        public static void FrogSpear_OnInteraction_Prefix(FrogSpear __instance, GameObject player)
        {
            SetWeaponOwner(__instance, player, "  FrogSpear");
        }

        [HarmonyPatch(typeof(WeaponHitDetection), "OnTriggerEnter")]
        [HarmonyPostfix]
        public static void OnTriggerEnter_Postfix(WeaponHitDetection __instance, Collider other)
        {
            try
            {
                if (!Input.GetKey(KeyCode.Mouse0)) return;
                if (__instance.swerd == null) return;

                if (!weaponOwners.TryGetValue(__instance.swerd, out GameObject owner))
                {
                    LoggerUtils.LogDebug("Swords_Patches", "No owner found for weapon");
                    return;
                }
                if (!NetworkUtils.IsLocalPlayerOwner(owner))
                {
                    return; // Non è il nostro player locale
                }
                if (__instance.swerd is MushroomSword)
                {
                    patchHandlerMushroomSword.ProcessHit(__instance.swerd.HitSubject, null);
                }
                else if (__instance.swerd is FrogBladeController)
                {
                    patchHandlerFrogBladeHandler.ProcessHit(__instance.swerd.HitSubject, null);
                }
                else if (__instance.swerd is ExcaliburController)
                {
                    patchHandlerExcaliburSword.ProcessHit(__instance.swerd.HitSubject, null);
                }
                else if (__instance.swerd is FrogSpear)
                {

                    patchHandlerFrogSpearHandler.ProcessHit(__instance.swerd.HitSubject, null);
                }
            }
            catch (Exception ex)
            {
                LoggerUtils.LogError("Swords_Patches", $"Patch error: {ex.Message}");
            }
        }




    }
}
