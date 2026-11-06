namespace ZooWorld.LevelSelect
{
    public sealed class LevelSelection : ILevelSelection
    {
        public string SelectedArenaId { get; private set; }

        public void Select(string arenaId) => SelectedArenaId = arenaId;
    }
}
