using System;
using System.Collections.Generic;
using R3;
using ZooWorld.Animals;
using ZooWorld.App;
using ZooWorld.Core;
using ZooWorld.Pooling;

namespace ZooWorld.Spawning
{
    public sealed class AnimalSpawner : IGameplayController, IDisposable
    {
        private readonly ISpawnTimer timer;
        private readonly ISpawnPositionProvider positionProvider;
        private readonly IAnimalDefinitionRegistry registry;
        private readonly IAnimalFactory factory;
        private readonly IPrefabPool<AnimalView> pool;
        private readonly IGameTicker ticker;
        private readonly List<AnimalHandle> spawned = new();

        private IDisposable subscription;
        private bool active;

        public AnimalSpawner(
            ISpawnTimer timer,
            ISpawnPositionProvider positionProvider,
            IAnimalDefinitionRegistry registry,
            IAnimalFactory factory,
            IPrefabPool<AnimalView> pool,
            IGameTicker ticker)
        {
            this.timer = timer;
            this.positionProvider = positionProvider;
            this.registry = registry;
            this.factory = factory;
            this.pool = pool;
            this.ticker = ticker;
        }

        public IReadOnlyList<AnimalHandle> Spawned => spawned;

        public event Action<AnimalHandle> AnimalSpawned;

        public void Begin()
        {
            if (active)
            {
                return;
            }

            active = true;
            timer.Reset();
            subscription = ticker.FixedUpdate.Subscribe(OnTick);
        }

        public void Stop()
        {
            if (!active)
            {
                return;
            }

            active = false;
            subscription?.Dispose();
            subscription = null;
            ClearSpawned();
        }

        public void Dispose() => Stop();

        private void OnTick(float deltaTime)
        {
            if (timer.Tick(deltaTime))
            {
                SpawnOne();
            }
        }

        private void SpawnOne()
        {
            var definition = registry.GetRandom();
            if (!positionProvider.TryGetSpawnPosition(definition, out var position))
            {
                return;
            }

            var handle = factory.Create(definition, position);
            spawned.Add(handle);
            AnimalSpawned?.Invoke(handle);
        }

        private void ClearSpawned()
        {
            foreach (var handle in spawned)
            {
                if (handle.IsAlive && handle.View != null)
                {
                    handle.View.Deactivate();
                    pool.Release(handle.View);
                }
            }

            spawned.Clear();
        }
    }
}
