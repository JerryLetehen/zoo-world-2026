using System;
using System.Collections.Generic;
using UnityEngine;
using ZooWorld.Arena;

namespace ZooWorld.LevelSelect
{
    public sealed class LevelSelectView : MonoBehaviour, ILevelSelectView
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private RectTransform buttonContainer;
        [SerializeField] private LevelButtonView buttonPrefab;

        private readonly List<LevelButtonView> buttons = new();
        private IArenaCatalog catalog;

        public event Action<string> ArenaSelected;

        [VContainer.Inject]
        public void Construct(IArenaCatalog catalog)
        {
            this.catalog = catalog;
        }

        public void Show()
        {
            RebuildButtons();
            if (panel != null)
            {
                panel.SetActive(true);
            }
        }

        public void Hide()
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }
        }

        private void RebuildButtons()
        {
            ClearButtons();
            if (buttonPrefab == null || buttonContainer == null)
            {
                return;
            }

            foreach (var entry in LevelListBuilder.Build(catalog))
            {
                var button = Instantiate(buttonPrefab, buttonContainer);
                button.gameObject.SetActive(true);
                var arenaId = entry.ArenaId;
                button.Bind(entry.Label, () => ArenaSelected?.Invoke(arenaId));
                buttons.Add(button);
            }
        }

        private void ClearButtons()
        {
            foreach (var button in buttons)
            {
                if (button != null)
                {
                    button.Unbind();
                    Destroy(button.gameObject);
                }
            }

            buttons.Clear();
        }

        private void OnDestroy() => ClearButtons();
    }
}
