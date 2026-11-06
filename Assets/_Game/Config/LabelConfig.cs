using System;
using UnityEngine;

namespace ZooWorld.Config
{
    [Serializable]
    public sealed class LabelConfig
    {
        public string Text = "Tasty!";
        [Min(0f)] public float Lifetime = 1f;
        [Min(0f)] public float FadeDuration = 0.3f;
    }
}
