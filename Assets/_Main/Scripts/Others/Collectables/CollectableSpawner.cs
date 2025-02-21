using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Shubham.Tyagi
{
    public class CollectableSpawner : MonoBehaviour
    {
        [SerializeField] private CollectableCoin prefab;
        [SerializeField] private float height = 3f;

        public Dictionary<int, CollectableCoin> collectableList { get; private set; } = new Dictionary<int, CollectableCoin>();
        private Platform platform;

        private void Awake() => platform = GetComponentInParent<Platform>();

        public void SpawnCollectables(List<Vector3> _positions)
        {
            ResetCollectables();
            // int _coinCount = Random.Range(minCount, maxCount + 1);

            // for (int i = 0; i < _coinCount; i++)
            // {
            //     float _laneX = lanePositions[Random.Range(0, lanePositions.Count)];
            //     float _zOffset = Random.Range(10f, 100f);
            //     Vector3 _spawnPos = new Vector3(_laneX, height, transform.position.z + _zOffset);
            //     collectableList.Add(Instantiate(prefab, _spawnPos, Quaternion.identity, parent));
            // }

            foreach (Vector3 _pos in _positions)
            {
                Vector3 _spawnPos = new Vector3(_pos.x + platform.offsetX, height, transform.position.z + _pos.z);
                CollectableCoin _collectable = Instantiate(prefab, _spawnPos, Quaternion.identity, platform.CollectableParent);
                _collectable.gameObject.layer = GameManager.Instance.GetLayer(platform.playerType);

                _collectable.SetId(collectableList.Count + 1);
                collectableList.Add(_collectable.id, _collectable);
                _collectable.OnCollected += OnCollected;
            }
        }

        private void OnCollected(int _id)
        {
            var _collectable = collectableList[_id];
            _collectable.OnCollected -= OnCollected;

            collectableList.Remove(_id);
            Destroy(_collectable.gameObject);
        }


        private void ResetCollectables()
        {
            foreach (var _collectable in collectableList)
                Destroy(_collectable.Value.gameObject);

            collectableList = new Dictionary<int, CollectableCoin>();
        }
    }
}