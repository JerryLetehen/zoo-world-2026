using TMPro;
using UnityEngine;

namespace ZooWorld.Scoring
{
    public sealed class ScoreView : MonoBehaviour
    {
        [SerializeField] private TMP_Text deadPreyText;
        [SerializeField] private TMP_Text deadPredatorsText;

        public void SetCounts(int deadPrey, int deadPredators)
        {
            if (deadPreyText != null)
            {
                deadPreyText.text = $"Dead prey: {deadPrey}";
            }

            if (deadPredatorsText != null)
            {
                deadPredatorsText.text = $"Dead predators: {deadPredators}";
            }
        }
    }
}
