using Pong.ClientObjects;
using Pong.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Pong.Managers
{
    public class ScoreboardManager : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Transform scoreboardContainer;

        [Header("Script References")]
        [SerializeField] private NetworkClientManager networkManager;

        [Header("Prefabs")]
        [SerializeField] private ScoreboardClient scoreboardPrefab;

        private readonly Dictionary<string, ScoreboardClient> localScoreBoard = new();
        private RectTransform scoreboardRect;

        private void Awake()
        {
            Assert.IsNotNull(scoreboardContainer);
            Assert.IsNotNull(networkManager);
        }

        // Start is called before the first frame update
        private void Start()
        {
            scoreboardRect = scoreboardContainer.GetComponent<RectTransform>();
            StartCoroutine(InitCoroutine());
        }

        private IEnumerator InitCoroutine()
        {
            yield return new WaitUntil(() => networkManager.GameRoom != null);

            networkManager.GameRoom.State.scoreboard.OnAdd((key, value) =>
            {
                AddScoreboardEntry(key);
            });

            networkManager.GameRoom.State.scoreboard.OnChange((key, value) =>
            {
                Debug.Log($"[ScoreboardManager] Player {key} Score: {value}");
                localScoreBoard.TryGetValue(key, out var scoreboardItem);
                if (scoreboardItem)
                {
                    scoreboardItem.UpdateScore(value);
                }
                else
                {
                    AddScoreboardEntry(key);
                    localScoreBoard[key].UpdateScore(value);
                }
            });
        }

        private void AddScoreboardEntry(string key)
        {
            Debug.Log("[ScoreboardManager] Adding scoreboard entry");
            var scoreboardItem = Instantiate(scoreboardPrefab, scoreboardContainer);
            LayoutRebuilder.MarkLayoutForRebuild(scoreboardRect);
            scoreboardItem.Init(networkManager.GameRoom.State.usernames[key]);
            localScoreBoard.Add(key, scoreboardItem);
        }
    }
}
