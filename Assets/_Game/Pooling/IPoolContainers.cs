using UnityEngine;

namespace ZooWorld.Pooling
{
    public interface IPoolContainers
    {
        Transform Get(PoolCategory category);
    }
}
