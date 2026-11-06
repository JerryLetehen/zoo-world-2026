using System;
using ZooWorld.Collision;
using ZooWorld.Pooling;

namespace ZooWorld.Animals
{
    public sealed class AnimalLifecycleService : IDisposable
    {
        private readonly ICollisionResolver resolver;
        private readonly IPrefabPool<AnimalView> pool;

        public AnimalLifecycleService(ICollisionResolver resolver, IPrefabPool<AnimalView> pool)
        {
            this.resolver = resolver;
            this.pool = pool;
            this.resolver.AnimalDied += OnAnimalDied;
        }

        private void OnAnimalDied(AnimalHandle handle)
        {
            if (handle.View == null)
            {
                return;
            }

            handle.View.Deactivate();
            pool.Release(handle.View);
        }

        public void Dispose()
        {
            resolver.AnimalDied -= OnAnimalDied;
        }
    }
}
