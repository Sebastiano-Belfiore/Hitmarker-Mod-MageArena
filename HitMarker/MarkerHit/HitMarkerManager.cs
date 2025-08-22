using HitMarker.Core;
using HitMarker.Utils;
using System;
using System.Collections;
using UnityEngine;

namespace HitMarker.MarkerHit
{
    public class HitMarkerManager : MonoBehaviour
    {
        private HitMarkerData _hitMarkerData;
        private HitMarkerBehaviour _hitMarkerBehaviour;
        private Texture2D _hitMarkerTexture;
        private WaitForSeconds _canvasWait;
        private GameObject canvas;

        public bool IsInitialized => _hitMarkerBehaviour != null;
        public GameObject Canvas => canvas;

        private void Awake()
        {
            _hitMarkerData = new HitMarkerData();
            _canvasWait = new WaitForSeconds(ModConstants.CANVAS_WAIT_TIME);
        }

        public void Initialize(Texture2D texture)
        {
            _hitMarkerTexture = texture;

            if (_hitMarkerTexture == null)
            {
                LoggerUtils.LogError("HitMarkerManager", "Cannot initialize with null texture");
                return;
            }

            LoggerUtils.LogInfo("HitMarkerManager", "Manager initialized with texture");
        }

        public void InitializeHitmarker()
        {
            if (_hitMarkerTexture == null)
            {
                LoggerUtils.LogError("HitMarkerManager", "Cannot initialize hitmarker: texture is null");
                return;
            }

            StartCoroutine(InitializeHitmarkerCoroutine());
        }

        public void ShowHitmarker()
        {
            if (!IsInitialized)
            {
                LoggerUtils.LogDebug("HitMarkerManager", "Cannot show hitmarker: not initialized");
                return;
            }

            try
            {
                _hitMarkerBehaviour.ShowHitMark();
                LoggerUtils.LogDebug("HitMarkerManager", "Hitmarker displayed");
            }
            catch (Exception ex)
            {
                LoggerUtils.LogError("HitMarkerManager", $"Failed to show hitmarker: {ex.Message}");
            }
        }

        private IEnumerator InitializeHitmarkerCoroutine()
        {
            LoggerUtils.LogInfo("HitMarkerManager", "Searching for Canvas...");

            GameObject canvasObj = null;
            int attempts = 0;
            const int maxAttempts = 15; // Reduced from 30

            while (canvasObj == null && attempts < maxAttempts)
            {
                canvasObj = GameObject.Find("Canvas");

                if (canvasObj == null)
                {
                    attempts++;
                    LoggerUtils.LogDebug("HitMarkerManager", $"Canvas search attempt {attempts}/{maxAttempts}");
                    yield return _canvasWait;
                }
            }

            if (canvasObj == null)
            {
                LoggerUtils.LogError("HitMarkerManager", $"Canvas not found after {maxAttempts} attempts");
                yield break;
            }
            canvas = canvasObj;
            LoggerUtils.LogInfo("HitMarkerManager", "Canvas found, creating hitmarker");
            CreateHitmarkerOnCanvas(canvasObj);
        }

        private void CreateHitmarkerOnCanvas(GameObject canvas)
        {
            try
            {

                GameObject hitmarkerObject = new GameObject("HitMarker");
                hitmarkerObject.transform.SetParent(canvas.transform, false);


                _hitMarkerBehaviour = hitmarkerObject.AddComponent<HitMarkerBehaviour>();


                Sprite hitmarkerSprite = ResourceLoader.CreateSpriteFromTexture(_hitMarkerTexture);
                if (hitmarkerSprite == null)
                {
                    LoggerUtils.LogError("HitMarkerManager", "Failed to create sprite from texture");
                    GameObject.Destroy(hitmarkerObject);
                    return;
                }


                _hitMarkerBehaviour.Initialize(_hitMarkerData, hitmarkerSprite);

                LoggerUtils.LogInfo("HitMarkerManager", "Hitmarker system ready");
            }
            catch (Exception ex)
            {
                LoggerUtils.LogCriticalError("HitMarkerManager", "Failed to create hitmarker on canvas", ex);
            }
        }
    }
}