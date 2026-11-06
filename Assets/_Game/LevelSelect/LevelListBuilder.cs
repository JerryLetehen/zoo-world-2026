using System.Collections.Generic;
using ZooWorld.Arena;

namespace ZooWorld.LevelSelect
{
    public static class LevelListBuilder
    {
        public static IReadOnlyList<LevelEntry> Build(IArenaCatalog catalog)
        {
            var entries = new List<LevelEntry>();
            foreach (var arenaId in catalog.ArenaIds)
            {
                catalog.TryGetDisplayName(arenaId, out var displayName);
                entries.Add(new LevelEntry(arenaId, displayName));
            }

            return entries;
        }
    }
}
