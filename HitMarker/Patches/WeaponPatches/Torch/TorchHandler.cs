using HitMarker.Patches;
using UnityEngine;

public class TorchHandler : BasePatchHandler
{
    public override string[] ValidTags => new[] { "Player", "PlayerNpc", "hitable", "wormhole", "brazier", "tutbrazier" };
    public override string PatchName => "Torch";

    public override bool MoreCheckHit(GameObject target)
    {
        return target != null && target.name.Contains("duende");
    }

    public override bool ProcessHit(GameObject target, GameObject owner)
    {
        return base.ProcessHit(target, owner);
    }
}