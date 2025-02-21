using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Shubham.Tyagi
{
    public class CollectableSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private Transform parent;
        [SerializeField] private float height = 1f;
        [SerializeField] private int minCount = 5, maxCount = 10;
        [SerializeField] private float minDistance = 10, maxDistance = 90;

        private List<GameObject> collectableList = new List<GameObject>();
        private List<float> lanePositions;

        private void Start() => SpawnCollectables();

        public void SpawnCollectables()
        {
            ResetCollectables();
            lanePositions = new() { -PlatformManager.Instance.LaneWidth, 0f, PlatformManager.Instance.LaneWidth };

            int _coinCount = Random.Range(minCount, maxCount + 1);
            // for (int i = 0; i < _coinCount; i++)
            // {
            //     float _laneX = lanePositions[Random.Range(0, lanePositions.Count)];
            //     float _zOffset = Random.Range(30f, 90f);
            //     Vector3 _spawnPos = new Vector3(_laneX, height, transform.position.z + _zOffset);
            //     collectableList.Add(Instantiate(prefab, _spawnPos, Quaternion.identity, parent));
            // }

            foreach (float _zPos in GenerateRandomIncreasingPositions(_coinCount))
            {
                float _laneX = lanePositions[Random.Range(0, lanePositions.Count)];
                Vector3 _spawnPos = new Vector3(_laneX, height, transform.position.z + _zPos);
                collectableList.Add(Instantiate(prefab, _spawnPos, Quaternion.identity, parent));
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


        private void ResetCollectables()
        {
            foreach (var _collectable in collectableList) Destroy(_collectable);
            collectableList = new List<GameObject>();
        }
    }
}