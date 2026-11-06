using VContainer.Unity;
using ZooWorld.LevelSelect;

namespace ZooWorld.App
{
    public sealed class GameEntryPoint : IStartable
    {
        private readonly IGameStateMachine stateMachine;

        public GameEntryPoint(IGameStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public void Start()
        {
            stateMachine.ChangeState<LevelSelectState>();
        }
    }
}
