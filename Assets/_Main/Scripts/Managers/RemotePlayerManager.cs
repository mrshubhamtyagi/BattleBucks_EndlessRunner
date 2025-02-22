using UnityEngine;

namespace Shubham.Tyagi
{
    public class RemotePlayerManager : MonoBehaviour
    {
        [SerializeField] private RemotePlayerController remotePlayer;
        [field: SerializeField] public float minDistanceToSendData = 0.1f;

        private Vector3 lastSentPosition;
        private bool lastJumpState;

        public static RemotePlayerManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null) return;
            Instance = this;
        }

        public void SetRemotePlayer(RemotePlayerController _player) => remotePlayer = _player;


        public void SendPlayerData(Vector3 position, bool jumped)
        {
            if (remotePlayer == null) return;
            // if (Vector3.Distance(position, lastSentPosition) < minDistanceToSendData && jumped == lastJumpState) return;

            // lastSentPosition = position;
            // lastJumpState = jumped;
            remotePlayer.ReceiveData(position.QuantizePosition(), jumped);
        }

        public void SendCollectedCoinData(int _id)
        {
            var _coin = PlatformManager.Instance.CurrentPlatformRemote.GetCollectedById(_id);
            if (_coin == null) return;

            print($"Remote Player Collected Coin {_coin.gameObject.name}");
            _coin.Collect();
        }

        public void SendObstacleData(int _id)
        {
            var _obstacle = PlatformManager.Instance.CurrentPlatformRemote.GetObstacleById(_id);
            if (_obstacle == null) return;
            print($"Remote Player Collided with {_obstacle.gameObject.name}");
            _obstacle.CollidedWithObstacle();
        }
    }
}