using HitMarker.Utils;
using UnityEngine;

namespace HitMarker.Patches.WeaponPatches
{
    public class SwordOwnerTracker : MonoBehaviour
    {
        public GameObject owner;
        public SwordController swordController;

        private void OnDestroy()
        {
            // Rimuovi automaticamente dal dictionary quando la spada viene distrutta
            if (swordController != null && Swords_Patches.weaponOwners.ContainsKey(swordController))
            {
                Swords_Patches.weaponOwners.Remove(swordController);
                LoggerUtils.LogDebug("SwordOwnerTracker", $"Auto-cleaned sword from dictionary on destroy");
            }
        }
    }

}
