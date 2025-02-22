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


        public void SendPlayerData(short[] _position, bool _hasJumped)
        {
            if (remotePlayer == null) return;
            // if (Vector3.Distance(position, lastSentPosition) < minDistanceToSendData && jumped == lastJumpState) return;

            // lastSentPosition = position;
            // lastJumpState = jumped;

            int _dataSize = (sizeof(short) * 3) + sizeof(bool);
            Log($"COMPRESSED Recieved - Position: {_position.ReveseQuantizePosition()} | Jump: {_hasJumped} | Data Size: {_dataSize} bytes");
            remotePlayer.ReceiveData(_position.ReveseQuantizePosition(), _hasJumped);
        }

        public void SendCollectedCoinData(int _id)
        {
            var _coin = PlatformManager.Instance.CurrentPlatformRemote.GetCollectedById(_id);
            if (_coin == null) return;

            UIManager.Instance.ShowLogRemote("Coin Collected!");
            print($"Remote Player Collected Coin {_coin.gameObject.name}");
            _coin.Collect();
        }

        public void SendObstacleData(int _id)
        {
            var _obstacle = PlatformManager.Instance.CurrentPlatformRemote.GetObstacleById(_id);
            if (_obstacle == null) return;

            UIManager.Instance.ShowLogRemote("Collided with Obstacle!");
            print($"Remote Player Collided with {_obstacle.gameObject.name}");
            _obstacle.CollidedWithObstacle();
        }

        private void Log(string _log)
        {
            if (GameManager.EnableLogInBuild || Application.isEditor)
                print(_log);
        }
    }
}