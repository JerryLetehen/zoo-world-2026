using System;
using UnityEngine;

namespace ZooWorld.Config
{
    [Serializable]
    public sealed class CameraConfig
    {
        [Min(0f)] public float Height = 25f;
        [Min(0f)] public float PanSpeed = 1f;
    }
}
