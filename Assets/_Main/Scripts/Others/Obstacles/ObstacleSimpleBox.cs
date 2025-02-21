using UnityEngine;

namespace Shubham.Tyagi
{
    public class ObstacleSimpleBox : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            print($"{gameObject.name} | {other.gameObject.name}");
            if (other.gameObject.CompareTag("Player"))
            {
                Debug.Log("Collided with Obstacle");
                GameManager.Instance.SetGameState(GameState.Ended);
            }
        }
    }
}