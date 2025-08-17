using HitMarkerMod.Patches;
using UnityEngine;

public class FrostBoltHandler : BasePatchHandler
{
    public override string[] ValidTags => new[] { "Player", "PlayerNpc", "hitable", "wormhole" };
    public override string PatchName => "FrostBolt";
    public override bool MoreCheckHit(GameObject target) => false;
}