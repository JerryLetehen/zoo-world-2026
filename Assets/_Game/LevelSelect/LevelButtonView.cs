using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ZooWorld.LevelSelect
{
    public sealed class LevelButtonView : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text label;

        public void Bind(string text, Action onClick)
        {
            label.text = text;
            button.onClick.AddListener(() => onClick());
        }

        public void Unbind() => button.onClick.RemoveAllListeners();
    }
}
