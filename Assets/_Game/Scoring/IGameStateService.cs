using System;

namespace ZooWorld.Scoring
{
    public interface IGameStateService
    {
        int DeadPrey { get; }
        int DeadPredators { get; }
        event Action CountsChanged;
    }
}
