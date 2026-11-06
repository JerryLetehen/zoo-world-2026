using UnityEngine;

namespace ZooWorld.Arena
{
    public interface IArenaInstance
    {
        string Id { get; }
        GameObject Root { get; }
        IArena Arena { get; }
    }
}
