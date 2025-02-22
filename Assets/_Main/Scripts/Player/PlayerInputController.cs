using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Shubham.Tyagi
{
    public class PlayerInputController : MonoBehaviour
    {
        private PlayerController playerController;

        public static PlayerInputController Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null) return;
            Instance = this;
        }

        void Start() => playerController = GetComponent<PlayerController>();

        void Update()
        {
            if (GameManager.Instance.GameState != GameState.Running) return;
            HandleInputs();
        }

        void HandleInputs()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
                Jump();
            else
            {
                if (Keyboard.current.leftArrowKey.wasPressedThisFrame || Keyboard.current.aKey.wasPressedThisFrame)
                    MoveLeft();
                else if (Keyboard.current.rightArrowKey.wasPressedThisFrame || Keyboard.current.dKey.wasPressedThisFrame)
                    MoveRight();
            }
        }


        public void MoveLeft() => playerController.MoveLeft();
        public void MoveRight() => playerController.MoveRight();
        public void Jump() => playerController.Jump();
    }
}