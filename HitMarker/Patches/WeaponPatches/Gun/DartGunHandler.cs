using UnityEngine;

namespace HitMarker.Patches.WeaponPatches.Gun
{
    public class DartGunHandler : BasePatchHandler
    {
        public override string[] ValidTags => new[] { "Player", "PlayerNpc", "hitable" };
        public override string PatchName => "DarkGun";
        public override bool MoreCheckHit(GameObject target)
        {
            return false;
        }

        public override bool ProcessHit(GameObject target, GameObject owner)
        {
            return base.ProcessHit(target, owner);
        }
    }
}
