using UnityEngine;

namespace HitMarker.Patches.WeaponPatches
{
    public class FrogSpearHandler : BasePatchHandler
    {
        public override string[] ValidTags => new[] { "Player", "hitable" };
        public override string PatchName => "FrogSpear"; //Frog Stick Wiki

        public override bool MoreCheckHit(GameObject target)
        {
            return false;
        }

        public override bool ProcessHit(GameObject target, GameObject owner)
        {
            // if (!IsLocalPlayerOwner(owner)) return false;
            return base.ProcessHit(target, owner);
        }
    }
}
