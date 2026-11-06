using System;

namespace ZooWorld.App
{
    public interface IStateResolver
    {
        IState Resolve(Type stateType);
    }
}
