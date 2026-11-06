using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace ZooWorld.Pooling
{
    public sealed class PrefabPool<T> : IPrefabPool<T> where T : Component
    {
        private readonly Transform container;
        private readonly Dictionary<T, ObjectPool<T>> poolsByPrefab = new();
        private readonly Dictionary<T, ObjectPool<T>> poolsByInstance = new();

        public PrefabPool(Transform container)
        {
            this.container = container;
        }

        public T Get(T prefab)
        {
            if (!poolsByPrefab.TryGetValue(prefab, out var pool))
            {
                pool = CreatePool(prefab);
                poolsByPrefab[prefab] = pool;
            }

            return pool.Get();
        }

        public void Release(T instance)
        {
            if (instance == null)
            {
                return;
            }

            if (!poolsByInstance.TryGetValue(instance, out var pool))
            {
                Debug.LogError(
                    $"Pool asked to release '{instance.name}', which it did not create. Destroying it to avoid corrupting a pool.");
                Object.Destroy(instance.gameObject);
                return;
            }

            pool.Release(instance);
        }

        private ObjectPool<T> CreatePool(T prefab)
        {
            ObjectPool<T> pool = null;
            pool = new ObjectPool<T>(
                createFunc: () => Create(prefab, pool),
                actionOnGet: instance => instance.gameObject.SetActive(true),
                actionOnRelease: instance => instance.gameObject.SetActive(false),
                actionOnDestroy: Destroy);
            return pool;
        }

        private T Create(T prefab, ObjectPool<T> pool)
        {
            var instance = Object.Instantiate(prefab, container);
            poolsByInstance[instance] = pool;
            return instance;
        }

        private void Destroy(T instance)
        {
            poolsByInstance.Remove(instance);
            Object.Destroy(instance.gameObject);
        }
    }
}
