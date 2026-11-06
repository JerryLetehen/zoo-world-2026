namespace ZooWorld.LevelSelect
{
    public interface ILevelSelection
    {
        string SelectedArenaId { get; }
        void Select(string arenaId);
    }
}
