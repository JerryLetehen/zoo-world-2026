using R3;
using UnityEngine;

namespace ZooWorld.Core
{
    public sealed class GameTicker : MonoBehaviour, IGameTicker
    {
        public Observable<float> Update { get; } =
            Observable.EveryUpdate(UnityFrameProvider.Update).Select(_ => Time.deltaTime);

        public Observable<float> FixedUpdate { get; } =
            Observable.EveryUpdate(UnityFrameProvider.FixedUpdate).Select(_ => Time.fixedDeltaTime);
    }
}
