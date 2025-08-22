using UnityEngine;

namespace HitMarker.Patches.WeaponPatches
{
    public class FrogBladeHandler : BasePatchHandler
    {
        public override string[] ValidTags => new[] { "Player", "PlayerNpc", "hitable" };
        public override string PatchName => "FrogBlade";

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
