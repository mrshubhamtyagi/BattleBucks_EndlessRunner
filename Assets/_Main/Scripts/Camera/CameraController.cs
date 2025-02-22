using System;
using UnityEngine;

namespace Shubham.Tyagi
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private float speed = 5f;
        [SerializeField] private Vector3 offset = new Vector3(0, 0, -10);
        [SerializeField] private LayerMask localLayerMask, remoteLayerMask;

        public float OffsetZ => offset.z;

        private Camera camera;
        private Vector3 startingPosition;
        private void Awake() => camera = GetComponentInChildren<Camera>();

        private void OnEnable() => GameManager.OnGameStateChanged += OnGameStateChanged;
        private void OnDisable() => GameManager.OnGameStateChanged -= OnGameStateChanged;

        public void SetPlayer(Transform _player, PlayerType _type, float _playerOffsetX, float _playerOffsetZ)
        {
            player = _player;
            Rect _rect = new Rect(0, 0, 1, 1);
            camera.cullingMask = _type == PlayerType.Local ? localLayerMask : remoteLayerMask;
            if (GameManager.Instance.gameMode == GameMode.Multiplayer)
            {
                _rect = _type == PlayerType.Local ? new Rect(0.5f, 0, 0.5f, 1) : new Rect(0, 0, 0.5f, 1);
                camera.GetComponent<AudioListener>().enabled = _type == PlayerType.Local;
            }

            camera.rect = _rect;
            startingPosition = new Vector3(_playerOffsetX, transform.position.y, offset.z + _playerOffsetZ);
            transform.position = startingPosition;
        }

        private void OnGameStateChanged(GameState _state)
        {
            if (_state == GameState.Ended)
                ResetPosition();
        }

        void LateUpdate()
        {
            if (GameManager.Instance.GameState != GameState.Running || player == null) return;
            if (player == null) return;

            Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y, player.position.z) + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
        }


        private void ResetPosition() => transform.position = startingPosition;
    }
}