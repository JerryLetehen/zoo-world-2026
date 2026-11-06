using System;
using UnityEngine;
using UnityEngine.UI;

namespace ZooWorld.App
{
    public sealed class GameplayHud : MonoBehaviour, IGameplayHud
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private Button backButton;

        public event Action BackRequested;

        private void Awake()
        {
            if (backButton != null)
            {
                backButton.onClick.AddListener(OnBackClicked);
            }
        }

        private void OnDestroy()
        {
            if (backButton != null)
            {
                backButton.onClick.RemoveListener(OnBackClicked);
            }
        }

        public void Show()
        {
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

        private void OnBackClicked() => BackRequested?.Invoke();
    }
}
