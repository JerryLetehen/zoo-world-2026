using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZooWorld.Arena
{
    public sealed class LocalArenaProvider : IArenaProvider
    {
        private readonly IArenaCatalog catalog;
        private readonly Func<ArenaRoot, ArenaRoot> instantiate;
        private readonly Action<GameObject> destroy;

        public LocalArenaProvider(
            IArenaCatalog catalog,
            Func<ArenaRoot, ArenaRoot> instantiate = null,
            Action<GameObject> destroy = null)
        {
            this.catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            this.instantiate = instantiate ?? UnityEngine.Object.Instantiate;
            this.destroy = destroy ?? UnityEngine.Object.Destroy;
        }

        public IArenaInstance Load(string arenaId)
        {
            if (!catalog.TryGetPrefab(arenaId, out var prefab) || prefab == null)
            {
                throw new ArgumentException(
                    $"No arena is registered for id '{arenaId}'. Known ids: {KnownIds()}.",
                    nameof(arenaId));
            }

            return new LoadedArena(arenaId, instantiate(prefab));
        }

        public void Unload(IArenaInstance arena)
        {
            if (arena?.Root != null)
            {
                destroy(arena.Root);
            }
        }

        private string KnownIds()
        {
            var ids = catalog.ArenaIds;
            return ids.Count > 0 ? string.Join(", ", ids) : "(none)";
        }

        private sealed class LoadedArena : IArenaInstance
        {
            private readonly ArenaRoot arenaRoot;
            private IArena arena;

            public LoadedArena(string id, ArenaRoot arenaRoot)
            {
                Id = id;
                this.arenaRoot = arenaRoot;
            }

            public string Id { get; }
            public GameObject Root => arenaRoot != null ? arenaRoot.gameObject : null;

            public IArena Arena => arena ??= arenaRoot != null ? arenaRoot.ToArena() : null;
        }
    }
}
