using FirstModProject.HitMarker;
using FirstModProject.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FirstModProject
{
    public class HitMarkerBehaviour : MonoBehaviour
    {
        private Image _hitmarkerImage;
        private RectTransform _rectTransform;
        private HitMarkerData _hitMarkerData;
        private bool _isAnimating = false;

     
        public void Initialize(HitMarkerData hitMarkerData, Sprite sprite)
        {
            //qui c'è un problema (Problema)
            /*
             Info   :  FirstMod] [FirstMod] [RESOURCE] Created sprite from texture (420x420)
[Info   :  FirstMod] [FirstMod] [HITMARKER] Initializing HitMarkerBehaviour
[Error  :  FirstMod] [FirstMod] [HitMarkerManager] Failed to create hitmarker on canvas: Object reference not set to an instance of an object*/

            //Guardando i log praticamente non arriva a 
            // Mod.Log.LogInfo("Init: HitMarker inizializzato e disattivato.");
            //quindi si interrompe qui 
            _hitMarkerData = hitMarkerData;

            LoggerUtils.LogHitmarker("Initializing HitMarkerBehaviour");


            SetupImageComponent(sprite);
            SetupRectTransform();

            

            
            this.gameObject.SetActive(false);

            LoggerUtils.LogHitmarker("Initializing HitMarkerBehaviour Finish");
        }
        private void SetupImageComponent(Sprite sprite)
        {
            LoggerUtils.LogHitmarker("Add Image componenet");
            _hitmarkerImage = gameObject.AddComponent<Image>();
            LoggerUtils.LogHitmarker("Added Image componenet");
            LoggerUtils.LogHitmarker("Add Sprite");
            _hitmarkerImage.sprite = sprite;
            LoggerUtils.LogHitmarker("Added Sprite");
            LoggerUtils.LogHitmarker("Add Color");
            _hitmarkerImage.color = _hitMarkerData.Color;
            LoggerUtils.LogHitmarker("Added Color");
        }
        private void SetupRectTransform()
        {
            LoggerUtils.LogHitmarker("Initializing RectTrasnfrom");
            LoggerUtils.LogHitmarker("Get RectTrasnfrom");
            _rectTransform = this.gameObject.GetComponent<RectTransform>();
            LoggerUtils.LogHitmarker("Getted RectTrasnfrom");
            _rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            _rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            _rectTransform.pivot = new Vector2(0.5f, 0.5f);
            _rectTransform.anchoredPosition = Vector2.zero;
            _rectTransform.sizeDelta = _hitMarkerData.Size;
            LoggerUtils.LogHitmarker("Finish RectTrasnfrom");
        }



        public void ShowHitMark()
        {
            if (_hitMarkerData == null || _hitmarkerImage == null)
            {
                LoggerUtils.LogWarning("HitMarkerBehaviour", "Cannot show hitmarker: not properly initialized");
                return;
            }

            LoggerUtils.LogHitmarker("ShowHitMark called, starting animation");

            gameObject.SetActive(true);

            // Ferma eventuali animazioni in corso
            if (_isAnimating)
            {
                StopAllCoroutines();
            }

            StartCoroutine(HitmarkerAnimationCoroutine());
        }


        private IEnumerator HitmarkerAnimationCoroutine()
        {
            _isAnimating = true;
            LoggerUtils.LogHitmarker("Animation coroutine started");

            
            float timer = 0f;
            Color startColor = _hitMarkerData.Color;
            startColor.a = 1f;
            _hitmarkerImage.color = startColor;
            _rectTransform.localScale = new Vector3(_hitMarkerData.ScaleStart, _hitMarkerData.ScaleStart, 1f);

          
            while (timer < _hitMarkerData.Duration)
            {
                float progress = timer / _hitMarkerData.Duration;

                //Scale
                float currentScale = Mathf.Lerp(_hitMarkerData.ScaleStart, _hitMarkerData.ScaleEnd, progress);
                _rectTransform.localScale = new Vector3(currentScale, currentScale, 1f);

                //Fade
                Color currentColor = _hitmarkerImage.color;
                currentColor.a = Mathf.Lerp(1f, 0f, progress);
                _hitmarkerImage.color = currentColor;

                timer += Time.deltaTime;
                yield return null;
            }

           
            _rectTransform.localScale = Vector3.one;
            Color finalColor = _hitmarkerImage.color;
            finalColor.a = 0f;
            _hitmarkerImage.color = finalColor;

            gameObject.SetActive(false);
            _isAnimating = false;

            LoggerUtils.LogHitmarker("Animation coroutine completed, GameObject deactivated");
        }
    }
}