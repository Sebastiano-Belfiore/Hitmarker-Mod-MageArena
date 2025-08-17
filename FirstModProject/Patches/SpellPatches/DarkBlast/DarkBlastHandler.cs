using HitMarkerMod.Patches;
using UnityEngine;

public class DarkBlastHandler : BasePatchHandler
{
    public override string[] ValidTags => new[] { "Player", "PlayerNpc", "hitable", "tutbrazier", "brazier", "wormhole" };
    public override string PatchName => "DarkBlast";

    public override bool MoreCheckHit(GameObject target)
    {
        return target != null && target.name.Contains("duende");
    }

    public override bool ProcessHit(GameObject target, GameObject owner)
    {
        if (!IsLocalPlayerOwner(owner)) return false;
        return base.ProcessHit(target, owner);
    }
}