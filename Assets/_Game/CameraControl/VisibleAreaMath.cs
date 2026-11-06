using System.Collections.Generic;
using UnityEngine;

namespace ZooWorld.CameraControl
{
    public static class VisibleAreaMath
    {
        public static Rect EnclosingXZRect(IReadOnlyList<Vector3> points)
        {
            var min = new Vector2(points[0].x, points[0].z);
            var max = min;
            for (var i = 1; i < points.Count; i++)
            {
                min = Vector2.Min(min, new Vector2(points[i].x, points[i].z));
                max = Vector2.Max(max, new Vector2(points[i].x, points[i].z));
            }

            return Rect.MinMaxRect(min.x, min.y, max.x, max.y);
        }

        public static bool ContainsXZ(Rect xzRect, Vector3 worldPosition)
        {
            return worldPosition.x >= xzRect.xMin && worldPosition.x <= xzRect.xMax
                && worldPosition.z >= xzRect.yMin && worldPosition.z <= xzRect.yMax;
        }
    }
}
