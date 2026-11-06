using UnityEngine;

namespace ZooWorld.CameraControl
{
    public interface ICameraPanInput
    {
        bool IsDragging { get; }
        Vector2 DragDelta { get; }
    }
}
