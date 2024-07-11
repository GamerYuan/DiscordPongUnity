using Pong.Networking;
using System.Collections;
using UnityEngine;

namespace Pong.Participants
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayerClient : ParticipantClient
    {
        [SerializeField] private float speed;

        private bool isMoving = false;
        private Vector2 targetPosition;
        private float startingX;

        private Player player;

        // Start is called before the first frame update
        private void Start()
        {
            startingX = transform.position.x;
        }

        public override void Init(string participantID, NetworkClientManager networkManager)
        {
            base.Init(participantID, networkManager);
            StartCoroutine(InitCoroutine());
        }

        private IEnumerator InitCoroutine()
        {
            yield return new WaitUntil(() => !string.IsNullOrEmpty(participantID) &&
                networkManager != null &&
                networkManager.GameRoom != null);

            player = networkManager.GameRoom.State.participants[participantID] as Player;

            player.OnYChange((y, _) =>
            {
                Debug.Log($"[PlayerClient] Player {participantID} Moved!");
                targetPosition = new Vector2(startingX, y / 10);
                isMoving = true;
            });

            Debug.Log("[PlayerClient] Player Initialized with ID: " + participantID);
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            if (participantID == networkManager.GameRoom.SessionId) HandleInput();

            if (isMoving && (Vector2)transform.position != targetPosition)
            {
                transform.position = targetPosition;
            }
            else
            {
                isMoving = false;
            }
        }

        private void HandleInput()
        {
            if (Input.GetKey(KeyCode.W))
            {
                networkManager.PlayerPosition(1);
            }

            if (Input.GetKey(KeyCode.S))
            {
                networkManager.PlayerPosition(-1);
            }
        }
    }
}
