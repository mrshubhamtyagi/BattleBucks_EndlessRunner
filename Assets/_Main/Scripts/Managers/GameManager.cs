using System;
using UnityEngine;

namespace Shubham.Tyagi
{
    public class GameManager : MonoBehaviour
    {
        [field: SerializeField] public GameMode gameMode { get; private set; } = GameMode.Multiplayer;
        [field: SerializeField] public GameState GameState { get; private set; } = GameState.NotStarted;
        [SerializeField] private int localLayerMask, remoteLayerMask;
        [SerializeField] private Light directionalLight;


        public static float DifficultyFactor => 0.2f;
        public static bool EnableLogInBuild => false;
        public static Action<GameState> OnGameStateChanged;
        public static Action OnDifficultyIncreased;
        public static GameManager Instance { get; private set; }


        private void Awake()
        {
            if (Instance != null) return;
            Instance = this;

            Application.targetFrameRate = 90;
        }

        public LayerMask GetLayer(PlayerType _type) => _type == PlayerType.Local ? localLayerMask : remoteLayerMask;

        public void SetGameState(GameState _state)
        {
            GameState = _state;
            OnGameStateChanged?.Invoke(GameState);

            if (_state == GameState.Running)
                InvokeRepeating(nameof(CheckForQualityAdjustment), 5, 5);
            else
                CancelInvoke(nameof(CheckForQualityAdjustment));
        }

        private void CheckForQualityAdjustment()
        {
            Log($"CheckForQualityAdjustment - {Application.targetFrameRate}");

            if (Application.targetFrameRate < 60)
                directionalLight.shadows = LightShadows.None;
            else if (Application.targetFrameRate < 75)
                directionalLight.shadows = LightShadows.Hard;
            else
                directionalLight.shadows = LightShadows.Soft;
        }

        public void IncreaseDifficulty() => OnDifficultyIncreased?.Invoke();

        private void Log(string _log)
        {
            if (GameManager.EnableLogInBuild || Application.isEditor)
                print(_log);
        }
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