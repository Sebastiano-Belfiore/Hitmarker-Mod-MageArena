using HitMarkerMod.Patches;
using UnityEngine;

public class LightningHandler : BasePatchHandler
{
    public override string[] ValidTags => new[] { "Player", "PlayerNpc", "hitable", "wormhole" };
    public override string PatchName => "Lightning";
    public override bool MoreCheckHit(GameObject target) => false;
}