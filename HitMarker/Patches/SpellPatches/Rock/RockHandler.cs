using UnityEngine;


namespace HitMarker.Patches.SpellPatches.Rock
{
    public class RockHandler : BasePatchHandler
    {
        public override string[] ValidTags => new[] { "Player", "PlayerNpc", "hitable", "wormhole", "brazier" };
        public override string PatchName => "Rock";
        public override bool MoreCheckHit(GameObject target) => false;
    }
}
