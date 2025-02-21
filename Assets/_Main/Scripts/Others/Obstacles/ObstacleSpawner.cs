using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Shubham.Tyagi
{
    public class ObstacleSpawner : MonoBehaviour
    {
        [SerializeField] private ObstacleSimpleBox prefab;
        [SerializeField] private float height = 1f;

        public Dictionary<int, ObstacleSimpleBox> obstacleList { get; private set; } = new Dictionary<int, ObstacleSimpleBox>();
        private Platform platform;

        private void Awake() => platform = GetComponentInParent<Platform>();
        // private void Start() => SpawnObstacles();

        public void SpawnObstacles(List<Vector3> _positions)
        {
            ResetObstacles();

            foreach (Vector3 _pos in _positions)
            {
                Vector3 _spawnPos = new Vector3(_pos.x + platform.offsetX, height, transform.position.z + _pos.z);
                ObstacleSimpleBox _obstacle = Instantiate(prefab, _spawnPos, Quaternion.identity, platform.ObstacleParent);
                _obstacle.gameObject.layer = GameManager.Instance.GetLayer(platform.playerType);

                _obstacle.SetId(obstacleList.Count + 1);
                obstacleList.Add(_obstacle.id, _obstacle);
                _obstacle.OnCollided += OnCollided;
            }
        }

        private void OnCollided(int _id)
        {
            var _obstacle = obstacleList[_id];
            _obstacle.OnCollided -= OnCollided;

            obstacleList.Remove(_id);
            Destroy(_obstacle.gameObject);
        }

        private void ResetObstacles()
        {
            foreach (var _obstacle in obstacleList)
                Destroy(_obstacle.Value.gameObject);

            obstacleList = new Dictionary<int, ObstacleSimpleBox>();
        }
    }
}