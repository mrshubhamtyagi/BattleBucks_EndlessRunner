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

        protected virtual void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            transform.position = startingPosition;
        }

        private void OnEnable() => GameManager.OnGameStateChanged += OnGameStateChanged;
        private void OnDisable() => GameManager.OnGameStateChanged -= OnGameStateChanged;

        protected virtual void OnGameStateChanged(GameState _state)
        {
        }

        protected virtual void FixedUpdate()
        {
            if (GameManager.Instance.GameState != GameState.Running) return;

            // MoveForward();
            ApplyGravity();
        }

        private void MoveForward() => rigidbody.linearVelocity = new Vector3(rigidbody.linearVelocity.x, rigidbody.linearVelocity.y, forwardSpeed);

        public void Jump()
        {
            if (!isGrounded) return;
            rigidbody.linearVelocity = new Vector3(rigidbody.linearVelocity.x, jumpForce, rigidbody.linearVelocity.z);
            isGrounded = false;
        }

        private void ApplyGravity()
        {
            if (isGrounded) return;
            rigidbody.linearVelocity -= Vector3.down * (gravity * Physics.gravity.y * Time.fixedDeltaTime);
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
                isGrounded = true;
        }
    }
}