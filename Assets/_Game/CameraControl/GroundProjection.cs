using UnityEngine;

namespace ZooWorld.CameraControl
{
    public static class GroundProjection
    {
        private static readonly Plane Ground = new Plane(Vector3.up, Vector3.zero);

        public static bool TryViewportToGround(Camera camera, Vector2 viewportPoint, out Vector3 groundPoint)
        {
            return TryRayToGround(camera.ViewportPointToRay(viewportPoint), out groundPoint);
        }

        public static bool TryScreenToGround(Camera camera, Vector2 screenPoint, out Vector3 groundPoint)
        {
            return TryRayToGround(camera.ScreenPointToRay(screenPoint), out groundPoint);
        }

        private static bool TryRayToGround(Ray ray, out Vector3 groundPoint)
        {
            if (Ground.Raycast(ray, out var distance))
            {
                groundPoint = ray.GetPoint(distance);
                return true;
            }

            groundPoint = default;
            return false;
        }
    }
}
