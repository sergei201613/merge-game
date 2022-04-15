using Gameplay.Common;
using TMPro;
using UnityEngine;

namespace Gameplay.UI
{
    public class HUDCanvas : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;

        private GameMode _gameMode;

        private void Awake()
        {
            _gameMode = FindObjectOfType<GameMode>();
        }

        private void OnEnable()
        {
            _gameMode.ScoreUpdated += UpdateScore;
        }

        private void OnDisable()
        {
            _gameMode.ScoreUpdated -= UpdateScore;
        }

        private void UpdateScore(int score)
        {
            scoreText.text = "Score: " + score;
        }
    }
}
