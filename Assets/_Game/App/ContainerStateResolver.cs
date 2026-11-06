using System;
using VContainer;

namespace ZooWorld.App
{
    public sealed class ContainerStateResolver : IStateResolver
    {
        private readonly IObjectResolver container;

        public ContainerStateResolver(IObjectResolver container)
        {
            this.container = container;
        }

        public IState Resolve(Type stateType) => (IState)container.Resolve(stateType);
    }
}
