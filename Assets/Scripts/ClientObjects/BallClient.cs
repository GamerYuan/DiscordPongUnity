using Pong.Networking;
using System.Collections;
using UnityEngine;

namespace Pong.ClientObjects
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class BallClient : MonoBehaviour
    {
        public NetworkClientManager networkManager;

        public Ball ball;

        private Vector2 targetPosition = Vector2.zero;

        private float speed = 12f;

        public void Init(NetworkClientManager networkManager)
        {
            this.networkManager = networkManager;
            StartCoroutine(InitCoroutine());
        }

        private IEnumerator InitCoroutine()
        {
            yield return new WaitUntil(() => networkManager != null &&
                networkManager.GameRoom != null);

            ball = networkManager.GameRoom.State.ball;

            ball.OnChange(() =>
            {
                if (ball.isNewBall)
                {
                    transform.position = Vector3.zero;
                    targetPosition = Vector2.zero;
                    Debug.Log("[BallClient] Ball Reset!");
                    return;
                }

                targetPosition = new Vector2(ball.x / 100, ball.y / 100);
                Debug.Log("[BallClient] Ball Position Changed!");
            });

            Debug.Log("[BallClient] Ball Initialized!");
        }

        private void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }
}
