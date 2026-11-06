using ZooWorld.App;

namespace ZooWorld.LevelSelect
{
    public sealed class LevelSelectState : IState
    {
        private readonly ILevelSelectView view;
        private readonly ILevelSelection selection;
        private readonly IGameStateMachine stateMachine;

        public LevelSelectState(
            ILevelSelectView view,
            ILevelSelection selection,
            IGameStateMachine stateMachine)
        {
            this.view = view;
            this.selection = selection;
            this.stateMachine = stateMachine;
        }

        public void Enter()
        {
            view.ArenaSelected += OnArenaSelected;
            view.Show();
        }

        public void Exit()
        {
            view.ArenaSelected -= OnArenaSelected;
            view.Hide();
        }

        private void OnArenaSelected(string arenaId)
        {
            selection.Select(arenaId);
            stateMachine.ChangeState<GameplayState>();
        }
    }
}
