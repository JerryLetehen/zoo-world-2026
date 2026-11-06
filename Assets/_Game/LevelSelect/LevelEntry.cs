namespace ZooWorld.LevelSelect
{
    public readonly struct LevelEntry
    {
        public LevelEntry(string arenaId, string label)
        {
            ArenaId = arenaId;
            Label = label;
        }

        public string ArenaId { get; }
        public string Label { get; }
    }
}
