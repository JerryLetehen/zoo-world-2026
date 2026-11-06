using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZooWorld.Arena
{
    public sealed class BoxArena : IArena
    {
        private readonly Bounds[] spawnRegions;

        public BoxArena(IReadOnlyList<Bounds> spawnRegions)
        {
            if (spawnRegions == null || spawnRegions.Count == 0)
            {
                throw new ArgumentException("An arena needs at least one spawn region.", nameof(spawnRegions));
            }

            this.spawnRegions = ToArray(spawnRegions);
        }

        public IReadOnlyList<Bounds> SpawnRegions => spawnRegions;

        public Vector3 ClampToInterior(Vector3 position, float margin)
        {
            var nearest = NearestRegion(position);
            var halfX = Mathf.Max(0f, nearest.extents.x - margin);
            var halfZ = Mathf.Max(0f, nearest.extents.z - margin);
            var x = Mathf.Clamp(position.x, nearest.center.x - halfX, nearest.center.x + halfX);
            var z = Mathf.Clamp(position.z, nearest.center.z - halfZ, nearest.center.z + halfZ);
            return new Vector3(x, position.y, z);
        }

        private Bounds NearestRegion(Vector3 position)
        {
            var nearest = spawnRegions[0];
            var best = SqrDistanceXZ(nearest, position);
            for (var i = 1; i < spawnRegions.Length; i++)
            {
                var distance = SqrDistanceXZ(spawnRegions[i], position);
                if (distance < best)
                {
                    best = distance;
                    nearest = spawnRegions[i];
                }
            }

            return nearest;
        }

        private static float SqrDistanceXZ(Bounds box, Vector3 position)
        {
            var flat = new Vector3(position.x, box.center.y, position.z);
            return (box.ClosestPoint(flat) - flat).sqrMagnitude;
        }

        private static Bounds[] ToArray(IReadOnlyList<Bounds> source)
        {
            var copy = new Bounds[source.Count];
            for (var i = 0; i < source.Count; i++)
            {
                copy[i] = source[i];
            }

            return copy;
        }
    }
}
