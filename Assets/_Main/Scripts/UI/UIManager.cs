using System;
using UnityEngine;

namespace Shubham.Tyagi
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject homeScreen, gameScreen, gameOverObj;
        [SerializeField] private TMPro.TextMeshProUGUI scoreText, finalScoreText;
        [SerializeField] private GameObject screenDivider, playerTags;

        public static UIManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null) return;
            Instance = this;
        }

        private void Start()
        {
            screenDivider.SetActive(GameManager.Instance.gameMode == GameMode.Multiplayer);
            playerTags.SetActive(GameManager.Instance.gameMode == GameMode.Multiplayer);
        }

        private void OnEnable() => GameManager.OnGameStateChanged += OnGameStateChanged;
        private void OnDisable() => GameManager.OnGameStateChanged -= OnGameStateChanged;

        public void UpdateScore(int _score) => scoreText.text = $"Collected: {_score}";


        public void OnClick_Play() => GameManager.Instance.SetGameState(GameState.Running);


        private void OnGameStateChanged(GameState _state)
        {
            switch (_state)
            {
                case GameState.Running:
                    UpdateScore(0);
                    gameOverObj.SetActive(false);
                    homeScreen.SetActive(false);
                    gameScreen.SetActive(true);
                    return;

                case GameState.Ended:
                    finalScoreText.text = scoreText.text;
                    gameOverObj.SetActive(true);
                    UpdateScore(0);
                    return;
            }
        }
    }
}