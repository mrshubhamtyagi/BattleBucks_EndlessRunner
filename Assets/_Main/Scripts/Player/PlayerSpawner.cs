using System;
using UnityEngine;

namespace Shubham.Tyagi
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private Player localPlayerPrefab, remotePlayerPrefab;
        [SerializeField] private CameraController cameraPrefab;

        public static PlayerSpawner Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null) return;
            Instance = this;
        }

        public void SpawnLocalPlayer()
        {
            PlayerController _local = Instantiate(localPlayerPrefab, null) as PlayerController;
            Instantiate(cameraPrefab, null).SetPlayer(_local.transform, true, _local.offsetX, _local.transform.position.z);
        }

        public void SpawnRemotePlayer()
        {
            RemotePlayerController _remote = Instantiate(remotePlayerPrefab, null) as RemotePlayerController;
            RemotePlayerManager.Instance.SetRemotePlayer(_remote);
            Instantiate(cameraPrefab, null).SetPlayer(_remote.transform, false, _remote.offsetX, _remote.transform.position.z);
        }
    }
}