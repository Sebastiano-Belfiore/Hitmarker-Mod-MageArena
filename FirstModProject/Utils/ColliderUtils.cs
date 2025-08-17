using FirstModProject.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FirstModProject.Utils
{
    public static class ColliderUtils
    {
        public static bool HasValidHitTag(Collider collider)
        {
            if (collider == null) return false;

            return ModConstants.VALID_HIT_TAGS.Contains(collider.tag);
        }
        public static bool HasValidHitTag(GameObject gameObject)
        {
            if (gameObject == null) return false;

            return ModConstants.VALID_HIT_TAGS.Contains(gameObject.tag);
        }
        public static bool HasValidHitLayer(GameObject gameObject)
        {
            if (gameObject == null) return false;

            return gameObject.layer == ModConstants.WALL_LAYER;
        }
        public static bool HasValidSpecialName(GameObject gameObject)
        {
            if (gameObject == null) return false;

            return ModConstants.SPECIAL_NAMES.Any(name => gameObject.name.Contains(name));
        }

        public static string GetTargetInfo(GameObject target)
        {
            if (target == null) return "Target is null";

            return $"Name: {target.name}, Tag: {target.tag}, Layer: {target.layer}";
        }
    }
}
