using System.Collections.Generic;
using UnityEngine;
using ZooWorld.Animals;
using ZooWorld.Spawning;

namespace ZooWorld.Collision
{
    public sealed class CollisionDispatcher
    {
        private readonly ICollisionResolver resolver;
        private readonly AnimalSpawner spawner;
        private readonly Dictionary<Rigidbody, AnimalHandle> handlesByBody = new();
        private readonly HashSet<long> resolvedThisContact = new();

        public CollisionDispatcher(ICollisionResolver resolver, AnimalSpawner spawner)
        {
            this.resolver = resolver;
            this.spawner = spawner;
            this.spawner.AnimalSpawned += OnAnimalSpawned;
        }

        private void OnAnimalSpawned(AnimalHandle handle)
        {
            if (handle.View == null)
            {
                return;
            }

            handlesByBody[handle.View.Body] = handle;
            handle.View.AnimalCollision -= OnAnimalCollision;
            handle.View.AnimalCollision += OnAnimalCollision;
        }

        private void OnAnimalCollision(AnimalView self, Rigidbody other)
        {
            if (!handlesByBody.TryGetValue(self.Body, out var a) || !handlesByBody.TryGetValue(other, out var b))
            {
                return;
            }

            if (!a.IsAlive || !b.IsAlive)
            {
                return;
            }

            var key = PairKey(a.Id, b.Id);
            if (!resolvedThisContact.Add(key))
            {
                return;
            }

            resolver.Resolve(a, b);
            resolvedThisContact.Remove(key);
        }

        private static long PairKey(int x, int y)
        {
            var low = x < y ? x : y;
            var high = x < y ? y : x;
            return ((long)low << 32) | (uint)high;
        }
    }
}
