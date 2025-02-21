using UnityEngine;

namespace Shubham.Tyagi
{
    public class Platform : MonoBehaviour
    {
        [SerializeField] private float speed = 10;
        private int platformLength => 100;

        private CollectableSpawner collectableSpawner;
        private ObstacleSpawner obstacleSpawner;

        private void Awake()
        {
            collectableSpawner = GetComponentInChildren<CollectableSpawner>();
            obstacleSpawner = GetComponentInChildren<ObstacleSpawner>();
        }


        private void Update()
        {
            if (GameManager.Instance.GameState != GameState.Running) return;
            MovePlatform();
        }


        private void MovePlatform()
        {
            transform.position -= Vector3.forward * (speed * Time.deltaTime);
            if (transform.position.z + platformLength < PlatformManager.Instance.CameraZOffset)
                RespawnPlatform();
        }


        private void RespawnPlatform()
        {
            if (collectableSpawner) collectableSpawner.SpawnCollectables();
            if (obstacleSpawner) obstacleSpawner.SpawnObstacles();
            PlatformManager.Instance.RespawnLastPlatform();
        }
    }
}