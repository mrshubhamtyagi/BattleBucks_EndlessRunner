using UnityEngine;
using UnityEngine.Serialization;

namespace Shubham.Tyagi
{
    public class RemotePlayerController : MonoBehaviour
    {
        [SerializeField] private float lerpSpeed = 5f;
        [SerializeField] private float jumpForce = 5f;
        [SerializeField] private float gravity = 2f;
        [SerializeField] private bool isGrounded;
        [SerializeField] private Vector3 startingPosition = Vector3.zero;

        private Rigidbody rigidbody;
        private Vector3 targetPosition;
        private float verticalVelocity = 0f;

        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            targetPosition = transform.position = startingPosition;
        }

        void FixedUpdate()
        {
            MovePlayer();
            ApplyGravity();
        }

        public void ReceiveMovementData(Vector3 _position, bool _hasJumped)
        {
            targetPosition = new Vector3(_position.x, transform.position.y, _position.z);

            if (_hasJumped)
            {
                Jump();
            }
        }

        void MovePlayer()
        {
            Vector3 _finalPos = Vector3.Lerp(transform.position, targetPosition, lerpSpeed * Time.deltaTime);
            rigidbody.linearVelocity = new Vector3(rigidbody.linearVelocity.x, jumpForce, rigidbody.linearVelocity.z);
        }

        void Jump()
        {
            if (!isGrounded) return;
            rigidbody.linearVelocity = new Vector3(rigidbody.linearVelocity.x, jumpForce, rigidbody.linearVelocity.z);
            isGrounded = false;
        }

        void ApplyGravity()
        {
            if (isGrounded) return;

            verticalVelocity -= gravity * Time.deltaTime;
            transform.position += Vector3.up * (verticalVelocity * Time.deltaTime);

            if (transform.position.y <= 0f)
            {
                transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
                isGrounded = true;
                verticalVelocity = 0f;
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = true;
            }
        }
    }
}