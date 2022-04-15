using UnityEngine;

namespace Gameplay.Common
{
    public class GameMode : MonoBehaviour
    {
        public int Score { get; private set; }

        public event System.Action<int> ScoreUpdated;

        public void AddScore(int value)
        {
            if (value <= 0)
                return;

            Score += value;
            ScoreUpdated?.Invoke(Score);
        }
    }
}
