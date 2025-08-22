using UnityEngine;

namespace HitMarker.Patches.WeaponPatches
{
    public class ExcaliburSwordHandler : BasePatchHandler
    {
        public override string[] ValidTags => new[] { "Player", "PlayerNpc", "hitable" };
        public override string PatchName => "ExcaliburSword";
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
