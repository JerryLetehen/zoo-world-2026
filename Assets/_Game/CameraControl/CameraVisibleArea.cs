using UnityEngine;

namespace ZooWorld.CameraControl
{
    public sealed class CameraVisibleArea : IVisibleArea
    {
        private static readonly Vector2[] ViewportCorners =
        {
            new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(0f, 1f), new Vector2(1f, 1f),
        };

        private readonly Camera camera;
        private readonly Vector3[] groundCorners = new Vector3[4];
        private CameraLens cachedLens;
        private bool hasCache;
        private bool cachedIsVisible;
        private Rect cachedRect;

        public CameraVisibleArea(Camera camera)
        {
            this.camera = camera;
        }

        public bool TryGetVisibleRect(out Rect xzRect)
        {
            if (camera == null)
            {
                xzRect = default;
                return false;
            }

            var lens = CameraLens.Of(camera);
            if (!hasCache || !lens.Equals(cachedLens))
            {
                cachedIsVisible = TryProjectVisibleRect(out cachedRect);
                cachedLens = lens;
                hasCache = true;
            }

            xzRect = cachedRect;
            return cachedIsVisible;
        }

        public bool Contains(Vector3 worldPosition)
        {
            return TryGetVisibleRect(out var rect) && VisibleAreaMath.ContainsXZ(rect, worldPosition);
        }

        private bool TryProjectVisibleRect(out Rect xzRect)
        {
            for (var i = 0; i < ViewportCorners.Length; i++)
            {
                if (!GroundProjection.TryViewportToGround(camera, ViewportCorners[i], out groundCorners[i]))
                {
                    xzRect = default;
                    return false;
                }
            }

            xzRect = VisibleAreaMath.EnclosingXZRect(groundCorners);
            return true;
        }

        private readonly struct CameraLens
        {
            private readonly Vector3 position;
            private readonly Quaternion rotation;
            private readonly bool orthographic;
            private readonly float orthographicSize;
            private readonly float fieldOfView;
            private readonly float aspect;

            private CameraLens(Camera camera)
            {
                var transform = camera.transform;
                position = transform.position;
                rotation = transform.rotation;
                orthographic = camera.orthographic;
                orthographicSize = camera.orthographicSize;
                fieldOfView = camera.fieldOfView;
                aspect = camera.aspect;
            }

            public static CameraLens Of(Camera camera) => new CameraLens(camera);

            public bool Equals(CameraLens other)
            {
                return position == other.position
                    && rotation == other.rotation
                    && orthographic == other.orthographic
                    && Mathf.Approximately(orthographicSize, other.orthographicSize)
                    && Mathf.Approximately(fieldOfView, other.fieldOfView)
                    && Mathf.Approximately(aspect, other.aspect);
            }
        }
    }
}
