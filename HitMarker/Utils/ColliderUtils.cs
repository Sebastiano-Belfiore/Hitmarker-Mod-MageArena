using UnityEngine;

namespace HitMarker.Utils
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
