using HitMarkerMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HitMarkerMod.Utils
{
    public static class ColliderUtils
    {
        public static string GetTargetInfo(GameObject target)
        {
            if (target == null) return "Target is null";

            return $"Name: {target.name}, Tag: {target.tag}, Layer: {target.layer}";
        }
    }
}
