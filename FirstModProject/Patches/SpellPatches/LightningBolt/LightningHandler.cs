using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FirstModProject.Patches.SpellPatches.LightningBolt
{
    public class LightningHandler : BasePatchHandler
    {
        public override string[] ValidTags => new[] { "Player", "PlayerNpc", "hitable", "wormhole" };

        public override string PatchName => "LightningBolt";

        public override bool MoreCheckHit(GameObject target)
        {
            return false;
        }

    }
}
