namespace ZooWorld.App
{
    public sealed class GameStateMachine : IGameStateMachine
    {
        private readonly IStateResolver resolver;

        public GameStateMachine(IStateResolver resolver)
        {
            this.resolver = resolver;
        }

        public IState Current { get; private set; }

        public void ChangeState<TState>() where TState : IState
        {
            Current?.Exit();
            Current = resolver.Resolve(typeof(TState));
            Current.Enter();
        }
    }
}
