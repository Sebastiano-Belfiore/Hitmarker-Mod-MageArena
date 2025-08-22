using HitMarker.Core;
using UnityEngine;



namespace HitMarker.Patches.WeaponPatches
{
    public class MushroomSwordHandler : BasePatchHandler
    {
        public override string[] ValidTags => new[] { "Player", "PlayerNpc", "hitable" };
        public override string PatchName => "MushroomSword";

        public override bool MoreCheckHit(GameObject target)
        {
            if (target == null) return false;
            return target.name.Contains("duende") || target.layer == ModConstants.WALL_LAYER;
        }

        public override bool ProcessHit(GameObject target, GameObject owner)
        {
            // if (!IsLocalPlayerOwner(owner)) return false;
            return base.ProcessHit(target, owner);
        }
    }
}
