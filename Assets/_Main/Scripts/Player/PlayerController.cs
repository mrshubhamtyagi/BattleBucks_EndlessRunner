using UnityEngine;

namespace Shubham.Tyagi
{
    public class PlayerController : Player
    {
        public float offsetX;
        private int _score;
        private Vector3 lastSentPosition;
        private bool lastSentJump;

        protected override void OnGameStateChanged(GameState _state)
        {
            base.OnGameStateChanged(_state);
            if (_state == GameState.Ended)
            {
                transform.position = startingPosition;
                currentLane = _score = 0;
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if (GameManager.Instance.GameState != GameState.Running) return;

            MoveLane();
            SendDataToRemotePlayer();
        }

        private void MoveLane()
        {
            Vector3 _targetPos = new Vector3(offsetX + (currentLane * PlatformManager.Instance.LaneWidth), transform.position.y, transform.position.z);
            Vector3 _finalPos = Vector3.Lerp(transform.position, _targetPos, Time.fixedDeltaTime * laneChangeSpeed);
            rigidbody.MovePosition(_finalPos);
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

        private void SendDataToRemotePlayer()
        {
            if (GameManager.Instance.gameMode == GameMode.SinglePlayer) return;

            if (ValidateChangeInData())
                RemotePlayerManager.Instance.SendPlayerData(lastSentPosition, lastSentJump);
        }

        private bool ValidateChangeInData()
        {
            bool _validChangeInPosition = Vector3.Distance(rigidbody.position, lastSentPosition) > RemotePlayerManager.Instance.minDistanceToSendData;

            bool _hasJumpedThisFrame = !isGrounded;
            bool _validChangeInJump = lastSentJump != _hasJumpedThisFrame;

            if (!_validChangeInPosition && !_validChangeInJump)
                return false;

            lastSentPosition = rigidbody.position.QuantizePosition();
            lastSentJump = _hasJumpedThisFrame;
            return true;
        }

        public void AddScore()
        {
            UIManager.Instance.UpdateScore(++_score);

            if (_score % 5 == 0)
                GameManager.Instance.IncreaseDifficulty();
        }
    }
}