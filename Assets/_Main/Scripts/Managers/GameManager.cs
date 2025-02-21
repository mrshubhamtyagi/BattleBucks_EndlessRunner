using System;
using UnityEngine;

namespace Shubham.Tyagi
{
    public class GameManager : MonoBehaviour
    {
        [field: SerializeField] public GameMode gameMode { get; private set; } = GameMode.Multiplayer;
        [field: SerializeField] public GameState GameState { get; private set; } = GameState.NotStarted;

        public static Action<GameState> OnGameStateChanged;
        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null) return;
            Instance = this;
        }

        public void SetGameState(GameState _state)
        {
            GameState = _state;
            OnGameStateChanged?.Invoke(GameState);
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
}