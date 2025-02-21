using System;
using UnityEngine;

namespace Shubham.Tyagi
{
    public class GameManager : MonoBehaviour
    {
        [field: SerializeField] public GameMode gameMode { get; private set; } = GameMode.Multiplayer;
        [field: SerializeField] public GameState GameState { get; private set; } = GameState.NotStarted;
        [SerializeField] private int localLayerMask, remoteLayerMask;

        public static Action<GameState> OnGameStateChanged;
        public static Action OnDifficultyIncreased;
        public static GameManager Instance { get; private set; }


        private void Awake()
        {
            if (Instance != null) return;
            Instance = this;
        }

        private void Start()
        {
            PlayerSpawner.Instance.SpawnLocalPlayer();
            PlatformManager.Instance.SpawnInitialPlatforms();
            if (gameMode == GameMode.Multiplayer)
            {
                PlayerSpawner.Instance.SpawnRemotePlayer();
            }
        }

        public LayerMask GetLayer(PlayerType _type) => _type == PlayerType.Local ? localLayerMask : remoteLayerMask;

        public void SetGameState(GameState _state)
        {
            GameState = _state;
            OnGameStateChanged?.Invoke(GameState);
        }

        public void IncreaseDifficulty() => OnDifficultyIncreased?.Invoke();
    }


    public enum GameState
    {
        NotStarted,
        Running,
        Ended,
    }

    public enum GameMode
    {
        SinglePlayer,
        Multiplayer,
    }

    public enum PlayerType
    {
        Local,
        Remote,
    }
}