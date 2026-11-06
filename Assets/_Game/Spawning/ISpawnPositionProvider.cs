using UnityEngine;
using ZooWorld.Animals;

namespace ZooWorld.Spawning
{
    public interface ISpawnPositionProvider
    {
        bool TryGetSpawnPosition(AnimalDefinition definition, out Vector3 position);
    }
}
