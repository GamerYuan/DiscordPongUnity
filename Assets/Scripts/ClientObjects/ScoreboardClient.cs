using TMPro;
using UnityEngine;

namespace Pong.ClientObjects
{
    public class ScoreboardClient : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text nameText;

        private string playerName;
        private int playerScore;

        public void Init(string name)
        {
            playerName = name;
            playerScore = 0;
            nameText.text = playerName;
            scoreText.text = playerScore.ToString();
        }

        public void UpdateScore(int score)
        {
            playerScore = score;
            scoreText.text = playerScore.ToString();
        }
    }
}
