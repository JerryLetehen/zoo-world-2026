using UnityEngine;

namespace ZooWorld.Config
{
    public abstract class ConfigAsset<T> : ScriptableObject where T : class, new()
    {
        [SerializeField] private T value = new();

        public T Value => value;
    }
}
