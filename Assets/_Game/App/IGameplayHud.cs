using System;

namespace ZooWorld.App
{
    public interface IGameplayHud
    {
        void Show();
        void Hide();
        event Action BackRequested;
    }
}
