using System;
using UnityEngine;

namespace Shubham.Tyagi
{
    public class CollectableCoin : MonoBehaviour
    {
        [field: SerializeField] public int id { get; private set; }

        public Action<int> OnCollected;

        public void SetId(int _id)
        {
            id = _id;
            gameObject.name = $"CollectableCoin_{_id}";
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerController _playerController))
            {
                UIManager.Instance.ShowLogLocal("Coin Collected!");

                _playerController.AddScore();
                if (GameManager.Instance.gameMode == GameMode.Multiplayer)
                    RemotePlayerManager.Instance.SendCollectedCoinData(id);
                Collect();
                // Invoke(nameof(CollectCoin), 0.5f);
            }
        }

        public void Collect()
        {
            // GetComponentInParent<Platform>().SpawnCoin();
            // Destroy(gameObject);
            OnCollected?.Invoke(id);
        }
    }
}