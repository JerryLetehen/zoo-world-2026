using System;
using UnityEngine;

namespace ZooWorld.Config
{
    [Serializable]
    public sealed class PhysicsConfig
    {
        public CollisionDetectionMode DetectionMode = CollisionDetectionMode.ContinuousDynamic;
        public RigidbodyInterpolation Interpolation = RigidbodyInterpolation.Interpolate;
        [Min(0.0001f)] public float FixedDeltaTime = 0.02f;
        [Range(0f, 1f)] public float Bounciness = 0.1f;
        [Range(0f, 1f)] public float Friction = 0.4f;
    }
}
