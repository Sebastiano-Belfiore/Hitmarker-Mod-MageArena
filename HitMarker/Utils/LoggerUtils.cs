using BepInEx.Logging;
using HitMarker.Core;
using System;

namespace HitMarker.Utils
{
    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error
    }

    public static class LoggerUtils
    {
        private static ManualLogSource _logger;

        // Configura qui il livello di log per la release
        // Debug: tutti i log, Info: solo info/warning/error, Warning: solo warning/error, Error: solo errori
        public static LogLevel MinLogLevel { get; set; } = LogLevel.Debug;

        public static void Initialize(ManualLogSource logger)
        {
            _logger = logger;

            // Log di inizializzazione sempre visibile
            _logger?.LogInfo($"{ModConstants.LOG_PREFIX} Logger initialized - Level: {MinLogLevel}");
        }

        private static bool ShouldLog(LogLevel level)
        {
            return level >= MinLogLevel;
        }

        // Metodi pubblici semplificati
        public static void LogInfo(string context, string message)
        {
            if (ShouldLog(LogLevel.Info))
                _logger?.LogInfo($"{ModConstants.LOG_PREFIX} [{context}] {message}");
        }

        public static void LogWarning(string context, string message)
        {
            if (ShouldLog(LogLevel.Warning))
                _logger?.LogWarning($"{ModConstants.LOG_PREFIX} [{context}] {message}");
        }

        public static void LogError(string context, string message)
        {
            if (ShouldLog(LogLevel.Error))
                _logger?.LogError($"{ModConstants.LOG_PREFIX} [{context}] {message}");
        }

        // Metodi specializzati - solo per debug
        public static void LogDebug(string context, string message)
        {
            if (ShouldLog(LogLevel.Debug))
                _logger?.LogInfo($"{ModConstants.LOG_PREFIX} [DEBUG] [{context}] {message}");
        }

        public static void LogPatch(string patchName, string message)
        {
            if (ShouldLog(LogLevel.Debug))
                _logger?.LogInfo($"{ModConstants.PATCH_LOG_PREFIX} [{patchName}] {message}");
        }

        public static void LogHitmarker(string message)
        {
            if (ShouldLog(LogLevel.Debug))
                _logger?.LogInfo($"{ModConstants.HITMARKER_LOG_PREFIX} {message}");
        }

        public static void LogResource(string message)
        {
            if (ShouldLog(LogLevel.Debug))
                _logger?.LogInfo($"{ModConstants.RESOURCE_LOG_PREFIX} {message}");
        }

        public static void LogHitDetection(string detectorName, string targetInfo, bool hitSuccessful)
        {
            if (ShouldLog(LogLevel.Debug))
            {
                string status = hitSuccessful ? "HIT" : "MISS";
                LogPatch(detectorName, $"{status} - {targetInfo}");
            }
        }
        public static void LogCriticalError(string context, string message, Exception ex = null)
        {
            string errorMsg = ex != null ? $"{message} - Exception: {ex.Message}" : message;
            _logger?.LogError($"{ModConstants.LOG_PREFIX} [CRITICAL] [{context}] {errorMsg}");
        }
    }
}