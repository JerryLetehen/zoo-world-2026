using UnityEngine;
using ZooWorld.Arena;

namespace ZooWorld.CameraControl
{
    public static class CameraPanMath
    {
        public static Vector3 ClampToBounds(Vector3 groundPoint, IArena bounds)
        {
            return bounds == null ? groundPoint : bounds.ClampToInterior(groundPoint, 0f);
        }
    }
}
