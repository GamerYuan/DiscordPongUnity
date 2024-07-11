using Pong.Networking;
using UnityEngine;

namespace Pong.Participants
{
    public abstract class ParticipantClient : MonoBehaviour
    {
        public string participantID;
        protected NetworkClientManager networkManager;

        public virtual void Init(string participantID, NetworkClientManager networkManager)
        {
            this.participantID = participantID;
            this.networkManager = networkManager;
        }
    }
}
