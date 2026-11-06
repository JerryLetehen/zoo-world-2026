using UnityEngine;

namespace ZooWorld.Pooling
{
    public interface IPrefabPool<T> where T : Component
    {
        T Get(T prefab);
        void Release(T instance);
    }
}
