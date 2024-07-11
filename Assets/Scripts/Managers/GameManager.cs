using Pong.Networking;
using Pong.Participants;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Pong.Managers
{
    public class GameManager : MonoBehaviour
    {
        [Header("Script References")]
        [SerializeField] private NetworkClientManager networkManager;

        [Header("Transforms")]
        [SerializeField] private Transform leftSpawnPoint;
        [SerializeField] private Transform rightSpawnPoint;

        [Header("Prefabs")]
        [SerializeField] private PlayerClient playerController;

        private readonly Dictionary<string, ParticipantClient> participantMap = new();
        private int playerNumber = -1;
        private bool isInitialized = false;

        private void Awake()
        {
            Assert.IsNotNull(networkManager);
        }

        private async void Start()
        {
            await networkManager.JoinOrCreate();

            networkManager.GameRoom.State.participants.ForEach(HandleParticipants);

            networkManager.GameRoom.State.participants.OnAdd((x, y) =>
            {
                Debug.Log("[GameManager] New Participant Joined!");
                HandleParticipants(x, y);
            });
        }

        private void HandleParticipants(string participantID, Participant participant)
        {
            if (participantID == networkManager.GameRoom.SessionId) HandleSelf();
            if (participantMap.ContainsKey(participantID)) return;

            Debug.Log($"[GameManager] Handling Participant {participantID}");

            if (participant is Player)
            {
                SpawnPlayer(playerNumber == 0 ? 1 : 0, out PlayerClient otherPlayer);
                otherPlayer.Init(participantID, networkManager);
                participantMap.Add(participantID, playerController);
            }
        }

        private void HandleSelf()
        {
            if (isInitialized) return;

            Debug.Log("[GameManager] Initializing Self");

            if (networkManager.GameRoom.State.participants.Count == 1)
                playerNumber = 0;
            else if (networkManager.GameRoom.State.participants.Count == 2)
                playerNumber = 1;

            SpawnPlayer(playerNumber, out PlayerClient playerObject);

            if (playerObject)
            {
                playerObject.Init(networkManager.GameRoom.SessionId, networkManager);
                participantMap.Add(networkManager.GameRoom.SessionId, playerObject);
            }

            isInitialized = true;
        }

        private void SpawnPlayer(int playerNumber, out PlayerClient player)
        {
            Debug.Log($"[GameManager] Spawning Player {playerNumber + 1}");

            if (playerNumber == 0) player = Instantiate(playerController, leftSpawnPoint.position, Quaternion.identity);
            else if (playerNumber == 1) player = Instantiate(playerController, rightSpawnPoint.position, Quaternion.identity);
            else player = null;
        }
    }
}
