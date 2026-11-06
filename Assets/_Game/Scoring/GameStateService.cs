using System;
using ZooWorld.Animals;
using ZooWorld.Collision;

namespace ZooWorld.Scoring
{
    public sealed class GameStateService : IGameStateService, IGameStateReset, IDisposable
    {
        private readonly ICollisionResolver resolver;

        public GameStateService(ICollisionResolver resolver)
        {
            this.resolver = resolver;
            this.resolver.AnimalDied += OnAnimalDied;
        }

        public int DeadPrey { get; private set; }
        public int DeadPredators { get; private set; }

        public event Action CountsChanged;

        private void OnAnimalDied(AnimalHandle handle)
        {
            if (handle.Definition.IsPredator)
            {
                DeadPredators++;
            }
            else
            {
                DeadPrey++;
            }

            CountsChanged?.Invoke();
        }

        public void Reset()
        {
            DeadPrey = 0;
            DeadPredators = 0;
            CountsChanged?.Invoke();
        }

        public void Dispose()
        {
            resolver.AnimalDied -= OnAnimalDied;
        }
    }
}
