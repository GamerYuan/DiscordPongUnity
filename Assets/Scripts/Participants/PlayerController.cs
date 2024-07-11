using Pong.Networking;
using System.Collections;
using UnityEngine;

namespace Pong.Participants
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayerMovement : Participant
    {
        [SerializeField] private float speed;
        [SerializeField] private KeyCode upKey;
        [SerializeField] private KeyCode downKey;

        private NetworkClientManager networkManager;

        private bool isMoving = false;
        private Vector2 targetPosition;

        private float startingX;
        private Player player;

        // Start is called before the first frame update
        private async void Start()
        {
            networkManager = gameObject.AddComponent<NetworkClientManager>();
            await networkManager.JoinOrCreate();
            startingX = transform.position.x;

            networkManager.GameRoom.OnMessage<string>("welcomeMessage", message =>
            {
                Debug.Log(message);
            });

            networkManager.GameRoom.State.OnChange(() =>
            {
                Debug.Log("Room State Changed");
                var player = networkManager.GameRoom.State.players[networkManager.GameRoom.SessionId];
                targetPosition = new Vector2(startingX, player.y);
                isMoving = true;
            });

            networkManager.GameRoom.State.players.OnAdd((key, player) =>
            {
                Debug.Log($"Player {key} has joined the Game!");
            });
            StartCoroutine(PlayerCoroutine());
        }

        private IEnumerator PlayerCoroutine()
        {
            yield return new WaitUntil(() => networkManager.GameRoom != null);
            yield return new WaitUntil(() => networkManager.GameRoom.State.players.ContainsKey(networkManager.GameRoom.SessionId));

            Debug.Log("WOW");
            player = networkManager.GameRoom.State.players[networkManager.GameRoom.SessionId];

            player.OnYChange((y, _) =>
            {
                Debug.Log("Player Y Changed");
                targetPosition = new Vector2(startingX, y);
                isMoving = true;
            });
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKey(upKey))
            {
                networkManager.PlayerPosition(transform.position.y + speed * Time.deltaTime);
            }

            if (Input.GetKey(downKey))
            {
                networkManager.PlayerPosition(transform.position.y - speed * Time.deltaTime);
            }

            if (isMoving && (Vector2)transform.position != targetPosition)
            {
                transform.position = targetPosition;
            }
            else
            {
                isMoving = false;
            }
        }
    }
}
