using ZooWorld.Arena;
using ZooWorld.LevelSelect;
using ZooWorld.Scoring;

namespace ZooWorld.App
{
    public sealed class GameplayState : IState
    {
        private readonly IGameplayController gameplay;
        private readonly IArenaProvider arenaProvider;
        private readonly IGameplayHud hud;
        private readonly ILevelSelection selection;
        private readonly IActiveArena activeArena;
        private readonly IGameStateMachine stateMachine;
        private readonly IGameStateReset gameStateReset;

        private IArenaInstance loadedArena;

        public GameplayState(
            IGameplayController gameplay,
            IArenaProvider arenaProvider,
            IGameplayHud hud,
            ILevelSelection selection,
            IActiveArena activeArena,
            IGameStateMachine stateMachine,
            IGameStateReset gameStateReset)
        {
            this.gameplay = gameplay;
            this.arenaProvider = arenaProvider;
            this.hud = hud;
            this.selection = selection;
            this.activeArena = activeArena;
            this.stateMachine = stateMachine;
            this.gameStateReset = gameStateReset;
        }

        public void Enter()
        {
            gameStateReset.Reset();
            loadedArena = arenaProvider.Load(selection.SelectedArenaId);
            activeArena.Set(loadedArena.Arena);
            hud.BackRequested += OnBackRequested;
            hud.Show();
            gameplay.Begin();
        }

        public void Exit()
        {
            gameplay.Stop();
            hud.BackRequested -= OnBackRequested;
            hud.Hide();
            activeArena.Clear();
            arenaProvider.Unload(loadedArena);
            loadedArena = null;
        }

        private void OnBackRequested() => stateMachine.ChangeState<LevelSelectState>();
    }
}
