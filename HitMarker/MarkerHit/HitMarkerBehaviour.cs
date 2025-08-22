using HitMarker.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace HitMarker.MarkerHit
{
    public class HitMarkerBehaviour : MonoBehaviour
    {
        private Image _hitmarkerImage;
        private RectTransform _rectTransform;
        private HitMarkerData _hitMarkerData;
        private bool _isAnimating = false;

        public void Initialize(HitMarkerData hitMarkerData, Sprite sprite)
        {
            _hitMarkerData = hitMarkerData;

            LoggerUtils.LogHitmarker("Initializing HitMarkerBehaviour");

            try
            {
                SetupImageComponent(sprite);
                SetupRectTransform();
                gameObject.SetActive(false);

                LoggerUtils.LogHitmarker("HitMarkerBehaviour initialized successfully");
            }
            catch (System.Exception ex)
            {
                LoggerUtils.LogCriticalError("HitMarkerBehaviour", "Failed to initialize", ex);
                throw;
            }
        }

        private void SetupImageComponent(Sprite sprite)
        {
            if (sprite == null)
            {
                LoggerUtils.LogError("HitMarkerBehaviour", "Cannot setup image with null sprite");
                return;
            }

            LoggerUtils.LogDebug("HitMarkerBehaviour", "Setting up Image component");

            _hitmarkerImage = gameObject.AddComponent<Image>();
            _hitmarkerImage.sprite = sprite;
            _hitmarkerImage.color = _hitMarkerData.Color;
        }

        private void SetupRectTransform()
        {
            LoggerUtils.LogDebug("HitMarkerBehaviour", "Setting up RectTransform");

            _rectTransform = GetComponent<RectTransform>();
            if (_rectTransform == null)
            {
                LoggerUtils.LogError("HitMarkerBehaviour", "RectTransform not found");
                return;
            }

            _rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            _rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            _rectTransform.pivot = new Vector2(0.5f, 0.5f);
            _rectTransform.anchoredPosition = Vector2.zero;
            _rectTransform.sizeDelta = _hitMarkerData.Size;
        }

        public void ShowHitMark()
        {
            if (_hitMarkerData == null || _hitmarkerImage == null)
            {
                LoggerUtils.LogWarning("HitMarkerBehaviour", "Cannot show hitmarker: not properly initialized");
                return;
            }

            LoggerUtils.LogDebug("HitMarkerBehaviour", "Showing hitmarker");

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


            float timer = 0f;
            Color startColor = _hitMarkerData.Color;
            startColor.a = 1f;
            _hitmarkerImage.color = startColor;
            _rectTransform.localScale = new Vector3(_hitMarkerData.ScaleStart, _hitMarkerData.ScaleStart, 1f);


            while (timer < _hitMarkerData.Duration)
            {
                float progress = timer / _hitMarkerData.Duration;

                // Scale animation
                float currentScale = Mathf.Lerp(_hitMarkerData.ScaleStart, _hitMarkerData.ScaleEnd, progress);
                _rectTransform.localScale = new Vector3(currentScale, currentScale, 1f);

                // Fade animation
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

            LoggerUtils.LogDebug("HitMarkerBehaviour", "Animation completed");
        }
    }
}