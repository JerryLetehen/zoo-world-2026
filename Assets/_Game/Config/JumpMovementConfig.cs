using System;
using UnityEngine;

namespace ZooWorld.Config
{
    [Serializable]
    public sealed class JumpMovementConfig
    {
        [Min(0f)] public float PeakHeight = 1.5f;
        [Min(0f)] public float HopDistance = 1.5f;
        [Min(0f)] public float RestSpeedThreshold = 0.05f;
    }
}
