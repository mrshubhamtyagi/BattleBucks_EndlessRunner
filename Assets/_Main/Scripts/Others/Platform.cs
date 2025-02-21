using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shubham.Tyagi
{
    public class Platform : MonoBehaviour
    {
        [SerializeField] private float speed = 10;
        public float offsetX = 10;
        public PlayerType playerType;

        private int platformLength => 100;
        private float intialSpeed = 10;

        private Transform collectablesParent, obstalcesParent;
        public Transform CollectableParent => collectablesParent;
        public Transform ObstacleParent => obstalcesParent;


        public CollectableSpawner collectableSpawner { get; private set; }
        public ObstacleSpawner obstacleSpawner { get; private set; }

        private void Awake()
        {
            collectablesParent = new GameObject("Collectables").transform;
            collectablesParent.parent = transform;
            obstalcesParent = new GameObject("Obstacles").transform;
            obstalcesParent.parent = transform;

            collectableSpawner = GetComponentInChildren<CollectableSpawner>();
            obstacleSpawner = GetComponentInChildren<ObstacleSpawner>();
        }

        private void Start()
        {
            intialSpeed = speed;
        }


        private void OnEnable()
        {
            GameManager.OnGameStateChanged += OnGameStateChanged;
            GameManager.OnDifficultyIncreased += OnIncreaseDifficulty;
        }

        private void OnDisable()
        {
            GameManager.OnGameStateChanged -= OnGameStateChanged;
            GameManager.OnDifficultyIncreased -= OnIncreaseDifficulty;
        }

        private void OnGameStateChanged(GameState _state)
        {
            if (_state == GameState.Ended)
                speed = intialSpeed;
        }

        private void OnIncreaseDifficulty() => speed += speed * .2f;

        public void Init(PlayerType _type, float _offsetX)
        {
            playerType = _type;
            offsetX = _offsetX;

            int _layer = GameManager.Instance.GetLayer(_type);
            foreach (Transform child in transform)
                child.gameObject.layer = _layer;
        }

        public void SpawnCollectables(List<Vector3> _positions) => collectableSpawner.SpawnCollectables(_positions);
        public void SpawnObstacles(List<Vector3> _positions) => obstacleSpawner.SpawnObstacles(_positions);

        private void Update()
        {
            if (GameManager.Instance.GameState != GameState.Running) return;
            MovePlatform();
        }


        private void MovePlatform()
        {
            transform.position -= Vector3.forward * (speed * Time.deltaTime);
            if (transform.position.z + platformLength < PlatformManager.Instance.CameraZOffset)
                PlatformManager.Instance.RespawnLastPlatform();
        }


        public CollectableCoin GetCollectedById(int _id) => collectableSpawner.collectableList.GetValueOrDefault(_id);
        public ObstacleSimpleBox GetObstacleById(int _id) => obstacleSpawner.obstacleList.GetValueOrDefault(_id);
    }
}