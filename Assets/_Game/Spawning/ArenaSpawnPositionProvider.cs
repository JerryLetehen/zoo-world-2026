using System;
using UnityEngine;
using VContainer;
using ZooWorld.Animals;
using ZooWorld.Arena;
using ZooWorld.CameraControl;
using ZooWorld.Config;

namespace ZooWorld.Spawning
{
    public sealed class ArenaSpawnPositionProvider : ISpawnPositionProvider
    {
        private readonly IActiveArena activeArena;
        private readonly IConfigProvider<SpawnConfig> config;
        private readonly IVisibleArea visibleArea;
        private readonly Func<float, float, float> randomRange;
        private readonly Func<int, int> randomIndex;
        private readonly Func<Vector3, float, bool> isBlocked;

        [Inject]
        public ArenaSpawnPositionProvider(
            IActiveArena activeArena,
            IConfigProvider<SpawnConfig> config,
            IVisibleArea visibleArea)
            : this(activeArena, config, visibleArea, null, null, null)
        {
        }

        public ArenaSpawnPositionProvider(
            IActiveArena activeArena,
            IConfigProvider<SpawnConfig> config,
            IVisibleArea visibleArea,
            Func<float, float, float> randomRange,
            Func<int, int> randomIndex,
            Func<Vector3, float, bool> isBlocked)
        {
            this.activeArena = activeArena;
            this.config = config;
            this.visibleArea = visibleArea;
            this.randomRange = randomRange ?? UnityEngine.Random.Range;
            this.randomIndex = randomIndex ?? DefaultIndex;
            this.isBlocked = isBlocked ?? DefaultBlockedCheck;
        }

        public bool TryGetSpawnPosition(AnimalDefinition definition, out Vector3 position)
        {
            var cfg = config.Get();
            var arena = activeArena.Current;
            var radius = Mathf.Max(0.1f, 0.5f);

            for (var attempt = 0; attempt < cfg.MaxPlacementAttempts; attempt++)
            {
                var region = arena.SpawnRegions[randomIndex(arena.SpawnRegions.Count)];
                var candidate = RandomPointIn(region, cfg.WallMargin, radius);

                if (visibleArea != null && !visibleArea.Contains(candidate))
                {
                    continue;
                }

                if (!isBlocked(candidate, radius))
                {
                    position = candidate;
                    return true;
                }
            }

            position = default;
            return false;
        }

        private Vector3 RandomPointIn(Bounds region, float margin, float radius)
        {
            var halfX = Mathf.Max(0f, region.extents.x - margin);
            var halfZ = Mathf.Max(0f, region.extents.z - margin);
            return new Vector3(
                region.center.x + randomRange(-halfX, halfX),
                region.min.y + radius,
                region.center.z + randomRange(-halfZ, halfZ));
        }

        private static int DefaultIndex(int count) => UnityEngine.Random.Range(0, count);

        private static bool DefaultBlockedCheck(Vector3 candidate, float radius)
        {
            return Physics.CheckSphere(
                candidate, radius * 0.9f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
        }
    }
}
