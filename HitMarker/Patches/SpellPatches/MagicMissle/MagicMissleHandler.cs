using UnityEngine;


namespace HitMarker.Patches.SpellPatches.MagicMissle
{
    public class MagicMissileHandler : BasePatchHandler
    {
        public override string[] ValidTags => new[] { "Player", "PlayerNpc", "hitable", "wormhole" };
        public override string PatchName => "MagicMissile";
        public override bool MoreCheckHit(GameObject target) => false;

        public override bool ProcessHit(GameObject target, GameObject owner)
        {
            if (!IsLocalPlayerOwner(owner)) return false;
            return base.ProcessHit(target, owner);
        }
    }
}
