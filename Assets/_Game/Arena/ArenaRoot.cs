using System.Collections.Generic;
using UnityEngine;

namespace ZooWorld.Arena
{
    public sealed class ArenaRoot : MonoBehaviour
    {
        [SerializeField] private List<BoxCollider> spawnRegions = new();

        public IArena ToArena()
        {
            return new BoxArena(ToWorld(spawnRegions));
        }

        private static Bounds[] ToWorld(List<BoxCollider> regions)
        {
            var world = new Bounds[regions.Count];
            for (var i = 0; i < regions.Count; i++)
            {
                var region = regions[i];
                world[i] = new Bounds(
                    region.transform.TransformPoint(region.center),
                    Vector3.Scale(region.size, region.transform.lossyScale));
            }

            return world;
        }
    }
}
