using BepInEx;
using HitMarkerMod.Core;
using HitMarkerMod.HitMarker;
using HitMarkerMod.Utils;
using HarmonyLib;
using System;
using System.Collections;
using UnityEngine;



namespace HitMarkerMod
{
    [BepInProcess("MageArena")]
    [BepInPlugin(ModConstants.MOD_GUID, ModConstants.MOD_NAME, ModConstants.MOD_VERSION)]
    public class Mod : BaseUnityPlugin
    {
        public static Mod Instance { get; private set; }

        private Texture2D _defaultHitmarkerTexture;
        private HitMarkerManager _hitMarkerManager;

      
        public HitMarkerManager HitMarkerManager => _hitMarkerManager;
        public Texture2D DefaultHitmarkerTexture => _defaultHitmarkerTexture;

        private void Awake()
        {
            // Singleton pattern
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                Initialize();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Initialize()
        {
            
            LoggerUtils.Initialize(Logger);

           
#if DEBUG
            LoggerUtils.MinLogLevel = LogLevel.Debug;
#else
            LoggerUtils.MinLogLevel = LogLevel.Info;
#endif

            LoggerUtils.LogInfo("Mod", $"HITMARKER MOD STARTUP - Version: {ModConstants.MOD_VERSION}");

            try
            {
                InitializeCore();
                ApplyHarmonyPatches();
                LoggerUtils.LogInfo("Mod", "Initialization completed successfully");
            }
            catch (Exception ex)
            {
                LoggerUtils.LogCriticalError("Mod", "Failed to initialize mod", ex);
            }
        }

        private void InitializeCore()
        {
            LoggerUtils.LogInfo("Mod", "Initializing core components...");

            // Carica texture hitmarker
            _defaultHitmarkerTexture = ResourceLoader.LoadTextureFromResources(ModConstants.HITMARKER_TEXTURE_PATH);
            if (_defaultHitmarkerTexture == null)
            {
                throw new Exception("Failed to load hitmarker texture - mod cannot function without it");
            }

            // Crea HitMarker Manager
            GameObject managerObject = new GameObject("HitMarkerManager");
            DontDestroyOnLoad(managerObject);
            _hitMarkerManager = managerObject.AddComponent<HitMarkerManager>();
            _hitMarkerManager.Initialize(_defaultHitmarkerTexture);

            LoggerUtils.LogDebug("Mod", "Core components initialized");
        }

        private void ApplyHarmonyPatches()
        {
            LoggerUtils.LogDebug("Mod", "Applying Harmony patches...");

            try
            {
                Harmony harmony = new Harmony(ModConstants.MOD_GUID);
                harmony.PatchAll();
                LoggerUtils.LogDebug("Mod", "Harmony patches applied successfully");
            }
            catch (Exception ex)
            {
                LoggerUtils.LogCriticalError("Mod", "Failed to apply Harmony patches", ex);
            }
        }

        public void ShowHitmarkerInstance()
        {
            if (_hitMarkerManager == null || !_hitMarkerManager.IsInitialized)
            {
                LoggerUtils.LogDebug("Mod", "Hitmarker not ready");
                return;
            }

            try
            {
                _hitMarkerManager.ShowHitmarker();
            }
            catch (Exception ex)
            {
                LoggerUtils.LogError("Mod", $"Failed to show hitmarker: {ex.Message}");
            }
        }

        public IEnumerator InitializeHitmarkerCoroutine(Texture2D texture)
        {
            if (texture == null || _hitMarkerManager == null)
            {
                LoggerUtils.LogError("Mod", "Cannot initialize hitmarker: invalid parameters");
                yield break;
            }

            LoggerUtils.LogInfo("Mod", "Starting hitmarker initialization...");

            _hitMarkerManager.InitializeHitmarker();

            
            float timeout = 10f;
            float elapsed = 0f;

            while (!_hitMarkerManager.IsInitialized && elapsed < timeout)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }

            if (_hitMarkerManager.IsInitialized)
            {
                LoggerUtils.LogInfo("Mod", "Hitmarker system ready");
            }
            else
            {
                LoggerUtils.LogError("Mod", "Hitmarker initialization timeout");
            }
        }

        private void OnDestroy()
        {
            LoggerUtils.LogInfo("Mod", "Mod shutting down");
        }
    }
}