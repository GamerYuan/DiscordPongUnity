using Colyseus;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

#if !UNITY_EDITOR
using static Dissonity.Api;
#endif

namespace Pong.Networking
{
    public class NetworkClientManager : MonoBehaviour
    {
        public ColyseusRoom<GameState> GameRoom => gameRoom;

        private ColyseusClient client;
        private ColyseusRoom<GameState> gameRoom;

        // Start is called before the first frame update
        private void Awake()
        {
#if UNITY_EDITOR
            client = new("ws://localhost:8080");
#else
            client = new("");
#endif
        }

        public async Task JoinOrCreate()
        {
#if UNITY_EDITOR
            string instanceId = "1";
            string userId = Random.Range(0, 500).ToString();
#else
            string instanceId = await GetSDKInstanceId();
            string userId = await GetUserId();
#endif
            gameRoom = await client.JoinOrCreate<GameState>("game", new Dictionary<string, object> { { "instanceId", instanceId }, { "userId", userId } });
        }

        public void PlayerPosition(float yPosition)
        {
            _ = gameRoom.Send("playerPosition", yPosition);
        }
    }
}
