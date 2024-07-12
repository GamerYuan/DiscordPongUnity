using Colyseus;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

#if !UNITY_EDITOR
using static Dissonity.Api;
using CandyCoded.env;
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
            if (env.TryParseEnvironmentVariable("APP_ID", out string serverUrl))
            {
                client = new($"wss://{serverUrl}.discordsays.com");
            }
            else
            {
                Debug.LogException(new System.Exception("App ID not found!"));
            }
#endif
        }

        public async Task JoinOrCreate()
        {
#if UNITY_EDITOR
            string instanceId = "1";
            string userId = Random.Range(0, 500).ToString();
            string displayName = "Tester " + userId;
#else
            string instanceId = await GetSDKInstanceId();
            string userId = await GetUserId();
            var user = await GetUser();
            string displayName = user.display_name;
#endif
            var rooms = await client.GetAvailableRooms("game");
            if (rooms.Any(x => x.roomId == instanceId))
            {
                Debug.Log($"[NetworkClientManager] Joining Existing Room with ID: {instanceId}");
                gameRoom = await client.JoinById<GameState>(instanceId, new Dictionary<string, object> { { "instanceId", instanceId }, { "userId", userId }, { "username", displayName } });
            }
            else
            {
                Debug.Log($"[NetworkClientManager] Creating New Room with ID: {instanceId}");
                gameRoom = await client.Create<GameState>("game", new Dictionary<string, object> { { "instanceId", instanceId }, { "userId", userId }, { "username", displayName } });
            }

            //gameRoom = await client.JoinOrCreate<GameState>("game", new Dictionary<string, object> { { "instanceId", instanceId }, { "userId", userId } });
        }

        public void PlayerPosition(float yPosition)
        {
            _ = gameRoom.Send("playerPosition", yPosition);
        }
    }
}
