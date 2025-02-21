using System;
using UnityEngine;

namespace Shubham.Tyagi
{
    public class RemotePlayerManager : MonoBehaviour
    {
        [SerializeField] private RemotePlayerController remotePlayer;

        private Vector3 lastSentPosition;
        private bool lastJumpState;

        public static RemotePlayerManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null) return;
            Instance = this;
        }


        public void SendPlayerData(Vector3 position, bool jumped)
        {
            if (Vector3.Distance(position, lastSentPosition) > 0.1f || jumped != lastJumpState)
            {
                lastSentPosition = position;
                lastJumpState = jumped;
                remotePlayer.ReceiveMovementData(position, jumped);
            }
        }
    }
}