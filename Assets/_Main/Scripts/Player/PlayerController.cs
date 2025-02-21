using UnityEngine;

namespace Shubham.Tyagi
{
    public class PlayerController : Player
    {
        // [SerializeField] private float forwardSpeed = 5f;
        // [SerializeField] private float laneChangeSpeed = 5f;
        // [SerializeField] private float jumpForce = 5f;
        // [SerializeField] private float gravity = 2f;
        // [SerializeField] private int currentLane = 0;
        // [SerializeField] private bool isGrounded;
        // [SerializeField] private Vector3 startingPosition = Vector3.zero;

        [SerializeField] private float minDistanceToSendData = 0.1f;

        // private Rigidbody rigidbody;
        private Vector3 lastSentPosition;
        private int _score;

        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            transform.position = lastSentPosition = startingPosition;
        }

        public override void OnGameStateChanged(GameState _state)
        {
            if (_state == GameState.Ended)
            {
                transform.position = lastSentPosition = startingPosition;
                currentLane = 0;
            }
        }


        void FixedUpdate()
        {
            if (GameManager.Instance.GameState != GameState.Running) return;

            // MoveForward();
            MoveLane();
            ApplyGravity();
            // SendDataToRemotePlayer();
        }

        void MoveForward() => rigidbody.linearVelocity = new Vector3(rigidbody.linearVelocity.x, rigidbody.linearVelocity.y, forwardSpeed);

        void MoveLane()
        {
            Vector3 targetPosition = new Vector3(currentLane * PlatformManager.Instance.LaneWidth, transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * laneChangeSpeed);
        }

        public void Jump()
        {
            if (!isGrounded) return;
            rigidbody.linearVelocity = new Vector3(rigidbody.linearVelocity.x, jumpForce, rigidbody.linearVelocity.z);
            isGrounded = false;
        }

        void ApplyGravity()
        {
            if (isGrounded) return;
            rigidbody.linearVelocity -= Vector3.down * (gravity * Physics.gravity.y * Time.deltaTime);
        }

        public void MoveLeft()
        {
            if (currentLane == -1) return;
            currentLane--;
        }

        public void MoveRight()
        {
            if (currentLane == 1) return;
            currentLane++;
        }


        void SendDataToRemotePlayer()
        {
            if (Vector3.Distance(transform.position, lastSentPosition) < minDistanceToSendData)
                return;

            lastSentPosition = transform.position;
            RemotePlayerManager.Instance.SendPlayerData(transform.position, !isGrounded);
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = true;
            }
        }


        public void AddScore() => UIManager.Instance.UpdateScore(++_score);
    }
}