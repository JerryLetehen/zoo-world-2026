using System;
using UnityEngine;

namespace ZooWorld.Config
{
    [Serializable]
    public sealed class LinearMovementConfig
    {
        [Min(0f)] public float Speed = 2.5f;
    }
}
