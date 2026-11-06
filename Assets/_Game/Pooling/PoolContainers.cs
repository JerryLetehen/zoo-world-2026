using System.Collections.Generic;
using UnityEngine;

namespace ZooWorld.Pooling
{
    public sealed class PoolContainers : IPoolContainers
    {
        private readonly Dictionary<PoolCategory, Transform> containers = new();

        public Transform Get(PoolCategory category)
        {
            if (!containers.TryGetValue(category, out var container) || container == null)
            {
                container = new GameObject($"Pool.{category}").transform;
                containers[category] = container;
            }

            return container;
        }
    }
}
