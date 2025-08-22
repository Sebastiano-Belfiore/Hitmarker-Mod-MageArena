using UnityEngine;


namespace HitMarker.Patches.SpellPatches.Fireball
{
    public class FireballHandler : BasePatchHandler
    {
        public override string[] ValidTags => new[] { "Player", "PlayerNpc", "hitable" };
        public override string PatchName => "Fireball";
        public override bool MoreCheckHit(GameObject target) => false;
    }
}
