namespace ZooWorld.App
{
    public interface IGameStateMachine
    {
        IState Current { get; }
        void ChangeState<TState>() where TState : IState;
    }
}
