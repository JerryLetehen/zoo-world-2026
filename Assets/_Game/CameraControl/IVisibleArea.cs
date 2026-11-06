using UnityEngine;

namespace ZooWorld.CameraControl
{
    public interface IVisibleArea
    {
        bool TryGetVisibleRect(out Rect xzRect);

        bool Contains(Vector3 worldPosition);
    }
}
