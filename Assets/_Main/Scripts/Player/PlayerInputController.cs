using UnityEngine;
using UnityEngine.InputSystem;
using TouchPhase = UnityEngine.TouchPhase;

namespace Shubham.Tyagi
{
    public class PlayerInputController : MonoBehaviour
    {
        private PlayerController playerController;

        void Start()
        {
            playerController = GetComponent<PlayerController>();
        }

        void Update()
        {
            if (GameManager.Instance.GameState != GameState.Running) return;
            HandleInputs();
        }

        void HandleInputs()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
                playerController.Jump();
            else
            {
                if (Keyboard.current.leftArrowKey.wasPressedThisFrame || Keyboard.current.aKey.wasPressedThisFrame)
                    playerController.MoveLeft();
                else if (Keyboard.current.rightArrowKey.wasPressedThisFrame ||
                         Keyboard.current.dKey.wasPressedThisFrame)
                    playerController.MoveRight();
            }
        }
    }
}