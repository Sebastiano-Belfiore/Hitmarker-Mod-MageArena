using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FirstModProject.Patches.SpellPatches.Rock
{
    internal class FireballHandler : BasePatchHandler
    {
        public override string[] ValidTags => new[] { "Player", "PlayerNpc", "hitable" };

        public override string PatchName => "Fireball";

        public override bool MoreCheckHit(GameObject target)
        {
            return false;
        }
    }
}
