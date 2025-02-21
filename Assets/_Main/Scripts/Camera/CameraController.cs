using System;
using UnityEngine;

namespace Shubham.Tyagi
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private float speed = 5f;
        [SerializeField] private Vector3 offset = new Vector3(0, 0, -10);
        [SerializeField] private LayerMask playerLayerMask, remoteLayerMask;

        public float OffsetZ => offset.z;

        private Camera camera;
        private Vector3 startingPosition;
        private void Awake() => camera = GetComponentInChildren<Camera>();

        private void OnEnable() => GameManager.OnGameStateChanged += OnGameStateChanged;
        private void OnDisable() => GameManager.OnGameStateChanged -= OnGameStateChanged;

        public void SetPlayer(Transform _player, bool _isLocalPlayer, float _playerOffsetX, float _playerOffsetZ)
        {
            player = _player;
            Rect _rect = new Rect(0, 0, 1, 1);
            if (GameManager.Instance.gameMode == GameMode.Multiplayer)
            {
                _rect = _isLocalPlayer ? new Rect(0.5f, 0, 0.5f, 1) : new Rect(0, 0, 0.5f, 1);
                camera.GetComponent<AudioListener>().enabled = _isLocalPlayer;
                camera.cullingMask = _isLocalPlayer ? playerLayerMask : remoteLayerMask;
            }

            camera.rect = _rect;
            startingPosition = new Vector3(_playerOffsetX, transform.position.y, offset.z + _playerOffsetZ);
            print(startingPosition);
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