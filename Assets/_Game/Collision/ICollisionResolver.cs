using System;
using ZooWorld.Animals;

namespace ZooWorld.Collision
{
    public interface ICollisionResolver
    {
        CollisionOutcome Resolve(AnimalHandle a, AnimalHandle b);
        event Action<AnimalHandle> AnimalDied;
        event Action<AnimalHandle> AnimalEaten;
    }
}
