using System;
using UnityEngine;
using ZooWorld.Animals;

namespace ZooWorld.Collision
{
    /// <summary>
    /// Resolves collisions by the two animals' diets: if neither eats the other they bounce, if one
    /// eats the other it does, and if each eats the other a random one wins. Already-dead handles
    /// are ignored.
    /// </summary>
    public sealed class FoodChainCollisionResolver : ICollisionResolver
    {
        public event Action<AnimalHandle> AnimalDied;
        public event Action<AnimalHandle> AnimalEaten;

        public CollisionOutcome Resolve(AnimalHandle a, AnimalHandle b)
        {
            if (a == null || b == null || !a.IsAlive || !b.IsAlive || ReferenceEquals(a, b))
            {
                return CollisionOutcome.None;
            }

            var aEatsB = a.Definition.CanEat(b.Definition);
            var bEatsA = b.Definition.CanEat(a.Definition);

            if (!aEatsB && !bEatsA)
            {
                return CollisionOutcome.Bounce;
            }

            var eater = ChooseEater(a, b, aEatsB, bEatsA);
            var eaten = ReferenceEquals(eater, a) ? b : a;
            Kill(eaten);
            AnimalEaten?.Invoke(eater);
            return CollisionOutcome.Eaten;
        }

        private static AnimalHandle ChooseEater(AnimalHandle a, AnimalHandle b, bool aEatsB, bool bEatsA)
        {
            if (aEatsB && bEatsA)
            {
                return UnityEngine.Random.value < 0.5f ? a : b;
            }

            return aEatsB ? a : b;
        }

        private void Kill(AnimalHandle handle)
        {
            handle.MarkDead();
            AnimalDied?.Invoke(handle);
        }
    }
}
