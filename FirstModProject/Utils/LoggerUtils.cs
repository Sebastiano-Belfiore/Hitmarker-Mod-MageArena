using BepInEx.Logging;
using FirstModProject.Core;
using Recognissimo.Samples.VoiceActivityDetectorExample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstModProject.Utils
{
    public static class LoggerUtils
    {
        private static ManualLogSource _logger;

        public static void Initialize(ManualLogSource logger)
        {
            _logger = logger;
        }

        public static void LogInfo(string context,string message)
        {
            _logger?.LogInfo($"{ModConstants.LOG_PREFIX} [{context}] {message}");
        }

        public static void LogWarning(string context, string message)
        {
            _logger?.LogWarning($"{ModConstants.LOG_PREFIX} [{context}] {message}");
        }
        public static void LogError(string context, string message)
        {
            _logger?.LogError($"{ModConstants.LOG_PREFIX} [{context}] {message}");
        }
        public static void LogPatch(string patchName, string message)
        {
            _logger?.LogInfo($"{ModConstants.PATCH_LOG_PREFIX} [{patchName}] {message}");
        }
        public static void LogHitmarker(string message)
        {
            _logger?.LogInfo($"{ModConstants.HITMARKER_LOG_PREFIX} {message}");
        }
        public static void LogResource(string message)
        {
            _logger?.LogInfo($"{ModConstants.RESOURCE_LOG_PREFIX} {message}");
        }
        public static void LogHitDetection(string detectorName, string targetInfo, bool hitSuccessful)
        {
            string status = hitSuccessful ? "HIT SUCCESSFUL" : "hit missed";
            LogPatch(detectorName, $"{status} - Target: {targetInfo}");
        }
    }
}
