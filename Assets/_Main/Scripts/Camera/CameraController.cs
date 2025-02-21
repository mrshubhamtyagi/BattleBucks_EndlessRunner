using System;
using UnityEngine;

namespace Shubham.Tyagi
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private float speed = 5f;
        [SerializeField] private Vector3 offset = new Vector3(0, 0, -10);

        public float OffsetZ => offset.z;

        private void Start()
        {
            Rect _rect = GameManager.Instance.gameMode == GameMode.SinglePlayer ? new Rect(0, 0, 1, 1) : new Rect(0.5f, 0, 0.5f, 1);
            GetComponentInChildren<Camera>().rect = _rect;
        }

        private void OnEnable() => GameManager.OnGameStateChanged += OnGameStateChanged;
        private void OnDisable() => GameManager.OnGameStateChanged -= OnGameStateChanged;

        private void OnGameStateChanged(GameState _state)
        {
            if (_state == GameState.Ended)
                ResetPosition();
        }

        void LateUpdate()
        {
            if (GameManager.Instance.GameState != GameState.Running) return;

            if (player == null) return;

            Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y, player.position.z) + offset;

            transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
        }


        private void ResetPosition() => transform.position = new Vector3(transform.position.x, transform.position.y, player.position.z) + offset;
    }
}