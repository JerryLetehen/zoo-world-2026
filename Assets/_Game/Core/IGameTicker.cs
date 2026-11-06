using R3;

namespace ZooWorld.Core
{
    public interface IGameTicker
    {
        Observable<float> Update { get; }
        Observable<float> FixedUpdate { get; }
    }
}
