using System;
using TMPro;
using UnityEngine;

namespace ZooWorld.Labels
{
    public sealed class TastyLabelView : MonoBehaviour
    {
        [SerializeField] private TextMeshPro text;

        private Action<TastyLabelView> expired;
        private float lifetime;
        private float fadeDuration;
        private float elapsed;
        private Vector3 startPosition;

        public void Show(
            string message,
            Vector3 worldPosition,
            float lifetime,
            float fadeDuration,
            Action<TastyLabelView> onExpired)
        {
            this.lifetime = Mathf.Max(0.01f, lifetime);
            this.fadeDuration = Mathf.Max(0f, fadeDuration);
            expired = onExpired;
            elapsed = 0f;
            startPosition = worldPosition;
            transform.position = worldPosition;

            text.text = message;
            SetAlpha(1f);
        }

        private void LateUpdate()
        {
            elapsed += Time.deltaTime;

            transform.position = startPosition + Vector3.up * (elapsed * 1.5f);
            FaceCamera();
            ApplyFade();

            if (elapsed >= lifetime)
            {
                Expire();
            }
        }

        private void Expire()
        {
            var callback = expired;
            expired = null;
            callback?.Invoke(this);
        }

        private void FaceCamera()
        {
            var cam = Camera.main;
            if (cam != null)
            {
                transform.rotation = cam.transform.rotation;
            }
        }

        private void ApplyFade()
        {
            if (fadeDuration <= 0f)
            {
                return;
            }

            var fadeStart = lifetime - fadeDuration;
            if (elapsed < fadeStart)
            {
                return;
            }

            var t = Mathf.InverseLerp(fadeStart, lifetime, elapsed);
            SetAlpha(Mathf.Clamp01(1f - t));
        }

        private void SetAlpha(float alpha)
        {
            var color = text.color;
            color.a = alpha;
            text.color = color;
        }
    }
}
