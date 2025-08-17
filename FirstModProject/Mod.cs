using BepInEx;
using BepInEx.Logging;
using FirstModProject.Core;
using FirstModProject.HitMarker;
using FirstModProject.Utils;
using FishNet.Object;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UI;


namespace FirstModProject
{
    [BepInPlugin(ModConstants.MOD_GUID, ModConstants.MOD_NAME, ModConstants.MOD_VERSION)]
    public class Mod : BaseUnityPlugin
    {

        public static Mod Instance { get; private set; }



        private const string modGUID = "First.Mod";
        public static ManualLogSource Log;

        private Texture2D _defaultHitmarkerTexture;
        private HitMarkerManager _hitMarkerManager;
        private WaitForSeconds _canvasWait;

        public HitMarkerBehaviour hitMarkerBehaviour;

        public HitMarkerManager HitMarkerManager => _hitMarkerManager;
        public Texture2D DefaultHitmarkerTexture => _defaultHitmarkerTexture;



        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }


            LoggerUtils.Initialize(Logger);
            LoggerUtils.LogInfo("Mod", " HITMARKER MOD STARTUP ");
            LoggerUtils.LogInfo("Mod", $"Version: {ModConstants.MOD_VERSION}");

            InitializeCore();

            ApplyHarmonyPatches();
        }
        private void OnDestroy()
        {
            LoggerUtils.LogInfo("Mod", "Shutting down...");

            // Shutdown API
            //HitMarkerAPI.Shutdown();

            LoggerUtils.LogInfo("Mod", "Shutdown complete");
        }
        private void InitializeCore()
        {
            LoggerUtils.LogInfo("Mod", "Initializing core components...");

            // Carica texture hitmarker
            _defaultHitmarkerTexture = ResourceLoader.LoadTextureFromResources(ModConstants.HITMARKER_TEXTURE_PATH);
            if (_defaultHitmarkerTexture == null)
            {
                LoggerUtils.LogError("Mod", "Failed to load hitmarker texture! Hitmarker functionality will be limited.");
                return;
            }


            GameObject managerObject = new GameObject("HitMarkerManager");
            DontDestroyOnLoad(managerObject);
            _hitMarkerManager = managerObject.AddComponent<HitMarkerManager>();
            _hitMarkerManager.Initialize(_defaultHitmarkerTexture);


            _canvasWait = new WaitForSeconds(ModConstants.CANVAS_WAIT_TIME);

            LoggerUtils.LogInfo("Mod", "Core components initialized");
        }
        private void ApplyHarmonyPatches()
        {
            LoggerUtils.LogInfo("Mod", "Applying Harmony patches for autonomous functionality...");

            try
            {
                Harmony harmony = new Harmony(ModConstants.MOD_GUID);
                harmony.PatchAll();
                LoggerUtils.LogInfo("Mod", "All Harmony patches applied successfully");
            }
            catch (Exception ex)
            {
                LoggerUtils.LogError("Mod", $"Failed to apply Harmony patches: {ex.Message}");
            }
        }
        public void ShowHitmarkerInstance()
        {
            if (_hitMarkerManager == null)
            {
                LoggerUtils.LogWarning("Mod", "Cannot show hitmarker: manager not initialized");
                return;
            }

            if (!_hitMarkerManager.IsInitialized)
            {
                LoggerUtils.LogWarning("Mod", "Cannot show hitmarker: manager not ready");
                return;
            }

            _hitMarkerManager.ShowHitmarker();
        }


        public IEnumerator InitializeHitmarkerCoroutine(Texture2D texture)
        {
            if (texture == null)
            {
                LoggerUtils.LogError("Mod", "Cannot initialize hitmarker with null texture");
                yield break;
            }

            LoggerUtils.LogInfo("Mod", "Starting hitmarker initialization...");


            _hitMarkerManager.InitializeHitmarker();


            float timeout = 5f;
            float elapsed = 0f;

            while (!_hitMarkerManager.IsInitialized && elapsed < timeout)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }

            if (_hitMarkerManager.IsInitialized)
            {
                LoggerUtils.LogInfo("Mod", "Hitmarker initialization completed successfully");

                // HitMarkerAPI.Initialize(_hitMarkerManager);
            }
            else
            {
                LoggerUtils.LogError("Mod", "Hitmarker initialization timed out");
            }
        }
    }
}

      

       


      /*  [HarmonyPatch(typeof(WispController), "Update")]
        public static class WispControllerPatch
        {
            static void Postfix(WispController __instance)
            {
                Mod.Log.LogInfo("[WispControllerPatch] Postfix avviato.");
                try
                {
                    // Accesso ai campi privati tramite Reflection
                    FieldInfo initedField = typeof(WispController).GetField("inited", BindingFlags.NonPublic | BindingFlags.Instance);
                    FieldInfo targetField = typeof(WispController).GetField("target", BindingFlags.NonPublic | BindingFlags.Instance);
                    FieldInfo ownerField = typeof(WispController).GetField("ownerob", BindingFlags.NonPublic | BindingFlags.Instance);

                    if (initedField == null || targetField == null || ownerField == null)
                    {
                        Mod.Log.LogWarning("[WispControllerPatch] Uno o più campi privati (inited, target, ownerob) in WispController non trovati tramite reflection.");
                        return;
                    }

                    bool inited = (bool)initedField.GetValue(__instance);
                    Transform target = (Transform)targetField.GetValue(__instance);

                    if (inited && target != null)
                    {
                        Mod.Log.LogInfo("[WispControllerPatch] Wisp è inizializzato e ha un bersaglio.");
                        if (Vector3.Distance(__instance.transform.position, target.position) < 1f)
                        {
                            Mod.Log.LogInfo("[WispControllerPatch] Wisp ha raggiunto il bersaglio. Verifico il proprietario.");
                            GameObject owner = (GameObject)ownerField.GetValue(__instance);
                            if (owner != null)
                            {
                                NetworkObject ownerNetworkObject = owner.GetComponent<NetworkObject>();
                                if (ownerNetworkObject != null && ownerNetworkObject.IsOwner)
                                {
                                    Mod.Log.LogInfo($"[WispControllerPatch] Sono il proprietario del Wisp. Colpo rilevato su '{target.name}'. Chiamo ShowHitmarkerInstance.");
                                    Mod.Instance.ShowHitmarkerInstance();
                                }
                                else
                                {
                                    Mod.Log.LogInfo("[WispControllerPatch] Il Wisp ha colpito un bersaglio, ma il proprietario non sono io.");
                                }
                            }
                            else
                            {
                                Mod.Log.LogInfo("[WispControllerPatch] Il proprietario del Wisp è null.");
                            }
                        }
                    }
                    else
                    {
                        Mod.Log.LogInfo("[WispControllerPatch] Wisp non inizializzato o senza bersaglio.");
                    }
                }
                catch (Exception ex)
                {
                    Mod.Log.LogError($"[WispControllerPatch] Errore nella patch: {ex.Message}");
                }
            }
        } */
          