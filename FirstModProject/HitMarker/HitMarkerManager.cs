using FirstModProject.Core;
using FirstModProject.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FirstModProject.HitMarker
{
    public class HitMarkerManager : MonoBehaviour
    {
        private HitMarkerData _hitMarkerData;
        private HitMarkerBehaviour _hitMarkerBehaviour;
        private Texture2D _hitMarkerTexture;
        private WaitForSeconds _canvasWait;

        public bool IsInitialized => _hitMarkerBehaviour != null;

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

            LoggerUtils.LogHitmarker("HitMarkerManager initialized successfully");
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
                LoggerUtils.LogWarning("HitMarkerManager", "Cannot show hitmarker: not initialized");
                return;
            }

            _hitMarkerBehaviour.ShowHitMark();
            LoggerUtils.LogHitmarker("Hitmarker displayed");
        }
        private IEnumerator InitializeHitmarkerCoroutine()
        {
            LoggerUtils.LogHitmarker("Starting canvas search...");

            GameObject canvasObj = null;
            int attempts = 0;
            const int maxAttempts = 30; // Limite per evitare loop infiniti

            while (canvasObj == null && attempts < maxAttempts)
            {
                canvasObj = GameObject.Find("Canvas");

                if (canvasObj == null)
                {
                    attempts++;
                    LoggerUtils.LogHitmarker($"Canvas not found, waiting... (attempt {attempts}/{maxAttempts})");
                    yield return _canvasWait;
                }
            }

            if (canvasObj == null)
            {
                LoggerUtils.LogError("HitMarkerManager", $"Canvas not found after {maxAttempts} attempts");
                yield break;
            }

            LoggerUtils.LogHitmarker($"Canvas '{canvasObj.name}' found!");
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

                LoggerUtils.LogHitmarker("Hitmarker GameObject created and initialized on Canvas");
            }
            catch (System.Exception ex)
            {
                LoggerUtils.LogError("HitMarkerManager", $"Failed to create hitmarker on canvas: {ex.Message}");
            }
        }
    }
}
