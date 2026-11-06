using UnityEngine;
using UnityEngine.InputSystem;

namespace ZooWorld.CameraControl
{
    public sealed class PointerPanInput : ICameraPanInput
    {
        public bool IsDragging =>
            Mouse.current != null && Mouse.current.leftButton.isPressed;

        public Vector2 DragDelta =>
            IsDragging && Mouse.current != null ? Mouse.current.delta.ReadValue() : Vector2.zero;
    }
}
