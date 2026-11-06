using System;
using UnityEngine;
using VContainer;
using ZooWorld.Config;

namespace ZooWorld.Spawning
{
    public sealed class RandomSpawnTimer : ISpawnTimer
    {
        private readonly IConfigProvider<SpawnConfig> config;
        private readonly Func<float, float, float> randomRange;
        private float timeRemaining;

        [Inject]
        public RandomSpawnTimer(IConfigProvider<SpawnConfig> config)
            : this(config, null)
        {
        }

        public RandomSpawnTimer(IConfigProvider<SpawnConfig> config, Func<float, float, float> randomRange)
        {
            this.config = config;
            this.randomRange = randomRange ?? UnityEngine.Random.Range;
            timeRemaining = NextInterval();
        }

        public bool Tick(float deltaTime)
        {
            timeRemaining -= deltaTime;
            if (timeRemaining > 0f)
            {
                return false;
            }

            timeRemaining += NextInterval();
            return true;
        }

        public void Reset()
        {
            timeRemaining = NextInterval();
        }

        private float NextInterval()
        {
            var cfg = config.Get();
            var min = Mathf.Min(cfg.MinInterval, cfg.MaxInterval);
            var max = Mathf.Max(cfg.MinInterval, cfg.MaxInterval);
            return randomRange(min, max);
        }
    }
}
