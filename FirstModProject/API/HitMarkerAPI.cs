using HitMarkerMod.HitMarker;
using HitMarkerMod.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HitMarkerMod.API
{
    public static class HitMarkerAPI
    {
        private static bool _isInitialized = false;

        /// <summary>
        /// Check if Hitmarker is available and ready
        /// </summary>
        public static bool IsAvailable => Mod.Instance != null && Mod.Instance.HitMarkerManager?.IsInitialized == true;


        /// <summary>
        /// Get the version of the Hitmarker mod
        /// </summary>
        public static string Version => Core.ModConstants.MOD_VERSION;


        /// <summary>
        /// Show a hitmarker with default settings
        /// Call this when your mod detects a hit
        /// </summary>
        /// <returns>True if hitmarker was shown, false if system not available</returns>
        public static bool ShowHitmarker()
        {
            if (!IsAvailable)
            {
                LoggerUtils.LogDebug("HitMarkerAPI", "Hitmarker system not available");
                return false;
            }

            try
            {
                Mod.Instance.ShowHitmarkerInstance();
                return true;
            }
            catch (System.Exception ex)
            {
                LoggerUtils.LogError("HitMarkerAPI", $"Failed to show hitmarker: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Check if the local player owns the specified GameObject
        /// </summary>
        /// <param name="gameObject">The GameObject to check ownership for</param>
        /// <returns>True if local player owns the object</returns>
        public static bool IsLocalPlayerOwner(GameObject gameObject)
        {
            return NetworkUtils.IsLocalPlayerOwner(gameObject);
        }
    }
}
