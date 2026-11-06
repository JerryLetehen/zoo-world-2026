namespace ZooWorld.Arena
{
    public interface IArenaProvider
    {
        IArenaInstance Load(string arenaId);
        void Unload(IArenaInstance arena);
    }
}
