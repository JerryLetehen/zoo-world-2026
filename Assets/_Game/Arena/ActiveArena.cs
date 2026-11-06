namespace ZooWorld.Arena
{
    public sealed class ActiveArena : IActiveArena
    {
        public IArena Current { get; private set; }

        public void Set(IArena arena) => Current = arena;

        public void Clear() => Current = null;
    }
}
