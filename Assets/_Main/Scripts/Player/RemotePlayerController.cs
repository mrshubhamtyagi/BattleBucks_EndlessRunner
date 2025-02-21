using UnityEngine;
using UnityEngine.Serialization;

namespace Shubham.Tyagi
{
    public class RemotePlayerController : Player
    {
        public float offsetX;
        [SerializeField] private Vector3 targetPosition;

        protected override void Start()
        {
            base.Start();
            targetPosition = startingPosition;
        }

        protected override void OnGameStateChanged(GameState _state)
        {
            base.OnGameStateChanged(_state);
            if (_state == GameState.Ended)
                transform.position = targetPosition = startingPosition;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if (GameManager.Instance.GameState != GameState.Running) return;

            MovePlayer();
        }

        public void ReceiveData(Vector3 _position, bool _hasJumped)
        {
            targetPosition = new Vector3(_position.x + offsetX, transform.position.y, _position.z);

            if (_hasJumped)
                Jump();
        }

        void MovePlayer()
        {
            Vector3 _finalPos = Vector3.Lerp(rigidbody.position, targetPosition, forwardSpeed * Time.fixedDeltaTime);
            rigidbody.MovePosition(_finalPos);
        }
    }
}