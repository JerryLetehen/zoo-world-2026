using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZooWorld.Arena
{
    [CreateAssetMenu(menuName = "ZooWorld/Arena Catalog", fileName = "ArenaCatalog")]
    public sealed class ArenaCatalog : ScriptableObject, IArenaCatalog
    {
        [Serializable]
        public struct Entry
        {
            public string Id;
            public ArenaRoot Prefab;
            public string DisplayName;
        }

        [SerializeField] private List<Entry> entries = new();

        public IReadOnlyList<string> ArenaIds
        {
            get
            {
                var ids = new List<string>(entries.Count);
                foreach (var entry in entries)
                {
                    ids.Add(entry.Id);
                }

                return ids;
            }
        }

        public bool TryGetDisplayName(string arenaId, out string displayName)
        {
            foreach (var entry in entries)
            {
                if (entry.Id == arenaId)
                {
                    displayName = entry.DisplayName;
                    return true;
                }
            }

            displayName = null;
            return false;
        }

        public bool TryGetPrefab(string arenaId, out ArenaRoot prefab)
        {
            foreach (var entry in entries)
            {
                if (entry.Id == arenaId)
                {
                    prefab = entry.Prefab;
                    return true;
                }
            }

            prefab = null;
            return false;
        }
    }
}
