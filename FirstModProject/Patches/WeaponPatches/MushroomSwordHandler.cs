using FirstModProject.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FirstModProject.Patches.WeaponPatches
{
    public class MushroomSwordHandler : BasePatchHandler
    {
        public override string[] ValidTags => new[] { "Player", "PlayerNpc", "hitable" };
        public override string PatchName => "MushroomSwordPatch";

        public override bool MoreCheckHit(GameObject target)
        {
            bool isValid1 = target.name.Contains("duende");
            bool isValid2 = target.layer == ModConstants.WALL_LAYER;
            return (isValid1 || isValid2);
        }
        public override bool ProcessHit(GameObject target, GameObject owner)
        {
            if (!IsLocalPlayerOwner(owner))
            {
                LogPatch("Weapon not owned by local player, skipping");
                return false;
            }
            return base.ProcessHit(target, owner);
        }
    }
}
