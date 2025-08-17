using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FirstModProject.Patches.SpellPatches.FrostBolt
{
    internal class FrostBoltHandler : BasePatchHandler
    {
        public override string[] ValidTags => new[] { "Player", "PlayerNpc", "hitable", "wormhole" };

        public override string PatchName => "FrostBolt";

        public override bool MoreCheckHit(GameObject target)
        {
            return false;
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
