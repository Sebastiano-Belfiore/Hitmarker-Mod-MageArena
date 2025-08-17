using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FirstModProject.Patches.SpellPatches.DarkBlast
{
    public class DarkBlastHandler : BasePatchHandler
    {
        public override string[] ValidTags => new[] { "Player", "PlayerNpc", "hitable", "tutbrazier", "brazier", "wormhole", };

        public override string PatchName => "DarkBlast";

        public override bool MoreCheckHit(GameObject target)
        {
           return target.name.Contains("duende");
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
