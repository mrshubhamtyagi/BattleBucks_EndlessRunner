using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Shubham.Tyagi
{
    public class ObstacleSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private Transform parent;
        [SerializeField] private float height = 1f;
        [SerializeField] private int minCount = 4, maxCount = 6;
        [SerializeField] private float minDistance = 10, maxDistance = 90;

        private List<GameObject> obstacleList = new List<GameObject>();
        private List<float> lanePositions;
        private Platform platform;

        private void Awake() => platform = GetComponentInParent<Platform>();

        private void Start() => SpawnObstacles();

        public void SpawnObstacles()
        {
            ResetObstacles();

            int _coinCount = Random.Range(minCount, maxCount + 1);
            lanePositions = new() { -PlatformManager.Instance.LaneWidth, 0f, PlatformManager.Instance.LaneWidth };
            foreach (float _zPos in GenerateRandomIncreasingPositions(_coinCount))
            {
                float _laneX = lanePositions[Random.Range(0, lanePositions.Count)];
                Vector3 _spawnPos = new Vector3(_laneX + platform.offsetX, height, transform.position.z + _zPos);
                obstacleList.Add(Instantiate(prefab, _spawnPos, Quaternion.identity, parent));
            }
        }

        private List<float> GenerateRandomIncreasingPositions(int _count)
        {
            List<float> _positions = new List<float>();
            float _currentZ = minDistance;
            float _remainingDistance = maxDistance - minDistance;
            float _stepMin = _remainingDistance / (_count * 2);
            float _stepMax = _remainingDistance / _count;

            for (int i = 0; i < _count; i++)
            {
                _currentZ += Random.Range(_stepMin, _stepMax);
                if (_currentZ > maxDistance) _currentZ = maxDistance;
                _positions.Add(_currentZ);
            }

            return _positions;
        }


        private void ResetObstacles()
        {
            foreach (var _collectable in obstacleList) Destroy(_collectable);
            obstacleList = new List<GameObject>();
        }
    }
}