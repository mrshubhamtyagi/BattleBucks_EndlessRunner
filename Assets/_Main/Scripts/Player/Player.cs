using System;
using UnityEngine;

namespace Shubham.Tyagi
{
    public abstract class Player : MonoBehaviour
    {
        [SerializeField] protected float forwardSpeed = 5f;
        [SerializeField] protected float laneChangeSpeed = 5f;
        [SerializeField] protected float jumpForce = 5f;
        [SerializeField] protected float gravity = 2f;
        [SerializeField] protected int currentLane = 0;
        [SerializeField] protected bool isGrounded;
        [SerializeField] protected Vector3 startingPosition = Vector3.zero;

        protected Rigidbody rigidbody;

        private void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            transform.position = startingPosition;
        }

        private void OnEnable() => GameManager.OnGameStateChanged += OnGameStateChanged;
        private void OnDisable() => GameManager.OnGameStateChanged -= OnGameStateChanged;

        public virtual void OnGameStateChanged(GameState _state)
        {
        }
    }
}