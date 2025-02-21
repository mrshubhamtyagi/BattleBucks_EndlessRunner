using UnityEngine;

namespace Shubham.Tyagi
{
    public class CollectableCoin : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Coin Collected!");
                if (other.TryGetComponent(out PlayerController _playerController))
                {
                    _playerController.AddScore();
                }

                Destroy(gameObject);
                // Invoke(nameof(CollectCoin), 0.5f);
            }
        }

        private void CollectCoin()
        {
            // GetComponentInParent<Platform>().SpawnCoin();
            Destroy(gameObject);
        }
    }
}