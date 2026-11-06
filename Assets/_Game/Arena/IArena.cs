using System.Collections.Generic;
using UnityEngine;

namespace ZooWorld.Arena
{
    public interface IArena
    {
        IReadOnlyList<Bounds> SpawnRegions { get; }
        Vector3 ClampToInterior(Vector3 position, float margin);
    }
}
