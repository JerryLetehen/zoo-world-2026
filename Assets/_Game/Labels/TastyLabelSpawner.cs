using System;
using UnityEngine;
using ZooWorld.Animals;
using ZooWorld.Collision;
using ZooWorld.Config;
using ZooWorld.Pooling;

namespace ZooWorld.Labels
{
    public sealed class TastyLabelSpawner : ITastyLabelSpawner, IDisposable
    {
        private readonly ICollisionResolver resolver;
        private readonly IConfigProvider<LabelConfig> config;
        private readonly IPrefabPool<TastyLabelView> pool;
        private readonly TastyLabelView prefab;

        public TastyLabelSpawner(
            ICollisionResolver resolver,
            IConfigProvider<LabelConfig> config,
            IPrefabPool<TastyLabelView> pool,
            TastyLabelView prefab)
        {
            this.resolver = resolver;
            this.config = config;
            this.pool = pool;
            this.prefab = prefab;
            this.resolver.AnimalEaten += Show;
        }

        public void Show(AnimalHandle predator)
        {
            if (predator?.View == null)
            {
                return;
            }

            var cfg = config.Get();
            var position = predator.Position + Vector3.up * 0.5f;

            var view = pool.Get(prefab);
            view.Show(cfg.Text, position, cfg.Lifetime, cfg.FadeDuration, pool.Release);
        }

        public void Dispose()
        {
            resolver.AnimalEaten -= Show;
        }
    }
}
