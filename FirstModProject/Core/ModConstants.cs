using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FirstModProject.Core
{
    public static class ModConstants
    {
        public const string MOD_GUID = "First.Mod";
        public const string MOD_NAME = "FirstMod";
        public const string MOD_VERSION = "0.1";

        public const string HITMARKER_TEXTURE_PATH = "FirstModProject.Resources.hitmarker.png";

        public static readonly string[] VALID_HIT_TAGS =
        {
              "Player",
            "PlayerNpc",
            "hitable",
            "tutbrazier",
            "brazier",
            "wormhole"
        };

        public const int WALL_LAYER = 3;

        public const float CANVAS_WAIT_TIME = 2f;
        public const float HITMARKER_DURATION = 0.5f;
        public const float HITMARKER_SCALE_START = 1.5f;
        public const float HITMARKER_SCALE_END = 1.0f;
        public const int HITMARKER_SIZE = 50;

        public static readonly Color HITMARKER_COLOR = new Color(1f, 0f, 0f, 1f);

        public static readonly string[] SPECIAL_NAMES = {
            "duende"
        };

        public const string LOG_PREFIX = "[FirstMod]";
        public const string PATCH_LOG_PREFIX = LOG_PREFIX + " [PATCH]";
        public const string HITMARKER_LOG_PREFIX = LOG_PREFIX + " [HITMARKER]";
        public const string RESOURCE_LOG_PREFIX = LOG_PREFIX + " [RESOURCE]";
    }
}
