using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FirstModProject.Patches.SpellPatches.Rock
{
    public class RockHandler : BasePatchHandler
    {
        public override string[] ValidTags => new[] { "Player", "PlayerNpc", "hitable", "wormhole", "brazier" };
        public override string PatchName => "Rock";
        public override bool MoreCheckHit(GameObject target)
        {
            return false;
        }
    }
}
