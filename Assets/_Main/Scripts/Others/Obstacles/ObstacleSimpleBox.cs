using System;
using UnityEngine;

namespace Shubham.Tyagi
{
    public class ObstacleSimpleBox : MonoBehaviour
    {
        [field: SerializeField] public int id { get; private set; }

        public Action<int> OnCollided;

        public void SetId(int _id)
        {
            id = _id;
            gameObject.name = $"ObstacleSimpleBox{_id}";
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out PlayerController _playerController))
            {
                UIManager.Instance.ShowLogLocal("Collided with Obstacle!");

                if (GameManager.Instance.gameMode == GameMode.Multiplayer)
                    RemotePlayerManager.Instance.SendObstacleData(id);
                GameManager.Instance.SetGameState(GameState.Ended);
            }
        }

        public void CollidedWithObstacle() => OnCollided?.Invoke(id);
    }
}