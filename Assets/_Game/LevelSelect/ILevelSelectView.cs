using System;

namespace ZooWorld.LevelSelect
{
    public interface ILevelSelectView
    {
        void Show();
        void Hide();
        event Action<string> ArenaSelected;
    }
}
