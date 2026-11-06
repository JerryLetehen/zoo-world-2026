using System;
using VContainer.Unity;

namespace ZooWorld.Scoring
{
    public sealed class ScorePresenter : IStartable, IDisposable
    {
        private readonly IGameStateService gameState;
        private readonly ScoreView view;

        public ScorePresenter(IGameStateService gameState, ScoreView view)
        {
            this.gameState = gameState;
            this.view = view;
        }

        public void Start()
        {
            gameState.CountsChanged += Refresh;
            Refresh();
        }

        public void Dispose()
        {
            gameState.CountsChanged -= Refresh;
        }

        private void Refresh()
        {
            if (view == null)
            {
                return;
            }

            view.SetCounts(gameState.DeadPrey, gameState.DeadPredators);
        }
    }
}
