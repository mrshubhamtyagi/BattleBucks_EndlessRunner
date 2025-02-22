using System.Collections.Generic;
using UnityEngine;

namespace Shubham.Tyagi
{
    public class PlatformManager : MonoBehaviour
    {
        [SerializeField] Platform platformPrefab;
        [SerializeField] Transform parent;
        [SerializeField] float offsetX;
        [field: SerializeField] public int LaneWidth { get; private set; } = 3;

        private int initialPlatformCount => 2;
        private int platformLength => 100;
        [SerializeField] private List<Platform> platformListLocal, platformListRemote;

        private int currentPlatformIndex = 0;
        private List<float> lanePositions;
        private CameraController cameraController;

        public float CameraZOffset => cameraController.OffsetZ - 2;
        public Platform CurrentPlatformLocal => platformListLocal[currentPlatformIndex];
        public Platform CurrentPlatformRemote => platformListRemote[currentPlatformIndex];

        public static PlatformManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null) return;
            Instance = this;
        }

        private void Start() => lanePositions = new() { -PlatformManager.Instance.LaneWidth, 0f, PlatformManager.Instance.LaneWidth };

        private void OnEnable() => GameManager.OnGameStateChanged += OnGameStateChanged;
        private void OnDisable() => GameManager.OnGameStateChanged -= OnGameStateChanged;

        private void OnGameStateChanged(GameState _state)
        {
            if (_state == GameState.Ended)
            {
                DestroyPlatformsLocal();
                DestroyPlatformsRemote();
                SpawnInitialPlatforms();
            }
        }

        public void SpawnInitialPlatforms()
        {
            if (cameraController == null)
                cameraController = FindFirstObjectByType<CameraController>();

            platformListLocal = new List<Platform>();
            platformListRemote = new List<Platform>();
            for (int i = 0; i < initialPlatformCount; i++)
            {
                Platform _local = SpawnPlatformLocal(i);
                var _collectablesPositions = GenerateRandomLanePositions(5, platformLength);
                var _obstaclesPositions = GenerateRandomLanePositions(10, platformLength);
                _local.SpawnCollectables(_collectablesPositions);
                _local.SpawnObstacles(_obstaclesPositions);

                if (GameManager.Instance.gameMode == GameMode.Multiplayer)
                {
                    Platform _remote = SpawnPlatformRemote(i);
                    _remote.SpawnCollectables(_collectablesPositions);
                    _remote.SpawnObstacles(_obstaclesPositions);
                }
            }
        }

        public void RespawnLastPlatform()
        {
            var _collectablesPositions = GenerateRandomLanePositions(0, platformLength);
            var _obstaclesPositions = GenerateRandomLanePositions(0, platformLength);
            RespawnLastPlatformLocal(CurrentPlatformLocal, _collectablesPositions, _obstaclesPositions);

            if (GameManager.Instance.gameMode == GameMode.Multiplayer)
                RespawnLastPlatformRemote(CurrentPlatformRemote, _collectablesPositions, _obstaclesPositions);

            currentPlatformIndex = (currentPlatformIndex + 1) % initialPlatformCount;
        }


        #region ------------------------------------------------------- LOCAL

        private void DestroyPlatformsLocal()
        {
            foreach (Platform _platform in platformListLocal)
                Destroy(_platform.gameObject);
        }

        private Platform SpawnPlatformLocal(int _i)
        {
            Vector3 _pos = Vector3.forward * _i * platformLength;
            _pos.z += CameraZOffset;
            _pos.x += offsetX;

            Platform _platform = Instantiate(platformPrefab, _pos, Quaternion.identity, parent);
            _platform.Init(PlayerType.Local, offsetX);
            platformListLocal.Add(_platform);

            _platform.name = $"Platform_{PlayerType.Local} {_i}";
            print($"SpawnLocalPlatform - {_platform.name}");
            return _platform;
        }

        private void RespawnLastPlatformLocal(Platform _platform, List<Vector3> _collectablesPositions, List<Vector3> _obstaclesPositions)
        {
            print($"RespawnLastPlatformLocal {_platform}");
            _platform.gameObject.SetActive(false);
            _platform.transform.position = new Vector3(_platform.offsetX, 0, platformLength + CameraZOffset);
            _platform.SpawnCollectables(_collectablesPositions);
            _platform.SpawnObstacles(_obstaclesPositions);
            _platform.gameObject.SetActive(true);
        }

        #endregion


        #region ------------------------------------------------------- REMOTE

        private void DestroyPlatformsRemote()
        {
            foreach (Platform _platform in platformListRemote)
                Destroy(_platform.gameObject);
        }

        private Platform SpawnPlatformRemote(int _i)
        {
            Vector3 _pos = Vector3.forward * _i * platformLength;
            _pos.z += CameraZOffset;
            _pos.x += -offsetX;

            Platform _platform = Instantiate(platformPrefab, _pos, Quaternion.identity, parent);
            _platform.Init(PlayerType.Remote, -offsetX);
            platformListRemote.Add(_platform);

            _platform.name = $"Platform_{PlayerType.Remote} {_i}";
            print($"SpawnRemotePlatform - {_platform.name}");
            return _platform;
        }

        private void RespawnLastPlatformRemote(Platform _platform, List<Vector3> _collectablesPositions, List<Vector3> _obstaclesPositions)
        {
            _platform.gameObject.SetActive(false);
            _platform.transform.position = new Vector3(_platform.offsetX, 0, platformLength + CameraZOffset);
            _platform.SpawnCollectables(_collectablesPositions);
            _platform.SpawnObstacles(_obstaclesPositions);
            _platform.gameObject.SetActive(true);
        }

        #endregion


        private List<Vector3> GenerateRandomLanePositions(float _minDistance, float _maxDistance)
        {
            List<Vector3> _positions = new();
            foreach (float _zPos in GenerateRandomIncreasingZPositions(_minDistance, _maxDistance))
            {
                float _laneX = lanePositions[Random.Range(0, lanePositions.Count)];
                Vector3 _spawnPos = new Vector3(_laneX, 0, transform.position.z + _zPos);
                _positions.Add(_spawnPos);
            }

            return _positions;
        }

        private List<float> GenerateRandomIncreasingZPositions(float _minDistance, float _maxDistance)
        {
            int _count = Random.Range(5, 8);
            List<float> _positions = new List<float>();
            float _currentZ = _minDistance;
            float _remainingDistance = _maxDistance - _minDistance;
            float _stepMin = _remainingDistance / (_count * 2);
            float _stepMax = _remainingDistance / _count;

            for (int i = 0; i < _count; i++)
            {
                _currentZ += Random.Range(_stepMin, _stepMax);
                if (_currentZ > _maxDistance) _currentZ = _maxDistance;
                _positions.Add(_currentZ);
            }

            return _positions;
        }
    }
}