using System;
using UnityEngine;

namespace ZooWorld.Config
{
    [Serializable]
    public sealed class SpawnConfig
    {
        [Min(0f)] public float MinInterval = 1f;
        [Min(0f)] public float MaxInterval = 2f;
        [Min(0f)] public float WallMargin = 0.5f;
        [Min(1)] public int MaxPlacementAttempts = 16;
    }
}
