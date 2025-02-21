using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shubham.Tyagi
{
    public class PlatformManager : MonoBehaviour
    {
        [SerializeField] Platform platformPrefab;
        [SerializeField] Transform parent;
        [field: SerializeField] public int LaneWidth { get; private set; } = 3;

        private int currentPlatformIndex = 0;
        private int platformCount => 2;
        private int platformLength => 100;
        private List<Platform> platformList;


        public float CameraZOffset => cameraController.OffsetZ - 2;
        public Platform CurrentPlatform => platformList[currentPlatformIndex];

        private CameraController cameraController;
        public static PlatformManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null) return;
            Instance = this;

            cameraController = FindFirstObjectByType<CameraController>();
        }

        private void Start() => SpawnInitialPlatforms();

        private void OnEnable() => GameManager.OnGameStateChanged += OnGameStateChanged;
        private void OnDisable() => GameManager.OnGameStateChanged -= OnGameStateChanged;

        private void OnGameStateChanged(GameState _state)
        {
            if (_state == GameState.Ended)
            {
                foreach (Platform _platform in platformList)
                    Destroy(_platform.gameObject);

                SpawnInitialPlatforms();
            }
        }

        private void SpawnInitialPlatforms()
        {
            platformList = new List<Platform>();
            for (int i = 0; i < platformCount; i++)
            {
                Vector3 _pos = Vector3.forward * i * platformLength;
                _pos.z += CameraZOffset;
                platformList.Add(Instantiate(platformPrefab, _pos, Quaternion.identity, parent));
            }
        }


        [ContextMenu("RespawnPlatform")]
        public void RespawnLastPlatform()
        {
            Platform _platform = CurrentPlatform;
            _platform.gameObject.SetActive(false);
            _platform.transform.position = Vector3.forward * (platformLength + CameraZOffset);
            _platform.gameObject.SetActive(true);
            currentPlatformIndex = ++currentPlatformIndex % platformCount;
        }
    }
}