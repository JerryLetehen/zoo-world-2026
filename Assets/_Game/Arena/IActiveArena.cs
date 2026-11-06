namespace ZooWorld.Arena
{
    public interface IActiveArena
    {
        IArena Current { get; }
        void Set(IArena arena);
        void Clear();
    }
}
