using System;
using UnityEngine;

namespace Shubham.Tyagi
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject homeScreen, gameScreen, gameOverObj;
        [SerializeField] private TMPro.TextMeshProUGUI scoreText, finalScoreText;
        [SerializeField] private GameObject screenDivider, playerTags;
        [SerializeField] private TMPro.TextMeshProUGUI localLogText, remoteLogText;

        private float hideLogTime = 1.5f;

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

        public void ShowLogLocal(string _text)
        {
            localLogText.text = $"[LOCAL]: {_text}";
            localLogText.gameObject.SetActive(true);
            Invoke(nameof(HideLogLocal), hideLogTime);
        }

        private void HideLogLocal() => localLogText.gameObject.SetActive(false);


        public void ShowLogRemote(string _text)
        {
            remoteLogText.text = $"[REMOTE]: {_text}";
            remoteLogText.gameObject.SetActive(true);
            Invoke(nameof(HideLogRemote), hideLogTime);
        }

        private void HideLogRemote() => remoteLogText.gameObject.SetActive(false);


        public void OnClick_Play() => GameManager.Instance.SetGameState(GameState.Running);

        public void OnClick_Restart()
        {
            GameManager.Instance.SetGameState(GameState.Ended);
            OnClick_Play();
        }

        public void OnClick_Left() => PlayerInputController.Instance.MoveLeft();
        public void OnClick_Right() => PlayerInputController.Instance.MoveRight();
        public void OnClick_Jump() => PlayerInputController.Instance.Jump();

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