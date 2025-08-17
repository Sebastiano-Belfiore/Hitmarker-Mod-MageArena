using UnityEngine;

namespace HitMarkerMod.Core
{
    public static class ModConstants
    {
       
        public const string MOD_GUID = "HitMarker.Mod";
        public const string MOD_NAME = "HitMarker";
        public const string MOD_VERSION = "0.1.0";

       
        public const string HITMARKER_TEXTURE_PATH = "HitMarkerMod.Resources.hitmarker.png";


        public static readonly string[] SPECIAL_NAMES = {
            "duende"
        };

        
        public const int WALL_LAYER = 3;

       
        public const float CANVAS_WAIT_TIME = 2f;
        public const float HITMARKER_DURATION = 0.5f;
        public const float HITMARKER_SCALE_START = 1.5f;
        public const float HITMARKER_SCALE_END = 1.0f;
        public const int HITMARKER_SIZE = 50;
        public static readonly Color HITMARKER_COLOR = new Color(1f, 0f, 0f, 1f);

        
        public const string LOG_PREFIX = "[HITMARKER]";
        public const string PATCH_LOG_PREFIX = LOG_PREFIX + " [PATCH]";
        public const string HITMARKER_LOG_PREFIX = LOG_PREFIX + " [HITMARKER]";
        public const string RESOURCE_LOG_PREFIX = LOG_PREFIX + " [RESOURCE]";
    }
}