using System.Collections.Generic;
using UnityEngine;

namespace ZooWorld.Arena
{
    public interface IArenaCatalog
    {
        IReadOnlyList<string> ArenaIds { get; }
        bool TryGetDisplayName(string arenaId, out string displayName);
        bool TryGetPrefab(string arenaId, out ArenaRoot prefab);
    }
}
