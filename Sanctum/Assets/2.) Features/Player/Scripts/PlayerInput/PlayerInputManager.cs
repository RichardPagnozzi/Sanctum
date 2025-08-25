using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInputManager : PlayerBase
    {
        #region Members

        private PlayerControls _playerInputActions;
        private PlayerCoordinator _playerCoordinator;
        private Vector2 _movementInput;
        private bool _isPaused;
        private bool _jumpPressed;


        public bool IsPaused
        {
            get => _isPaused;
            private set { _isPaused = value; }
        }

        public Vector2 MovementInput
        {
            get => _movementInput;
            set { _movementInput = value; }
        }

        #endregion

        #region Licecycle

        private void Awake()
        {
            _playerCoordinator = GetComponent<PlayerCoordinator>();
            if (TryGetPlayerDetails())
            {
                _playerInputActions = new PlayerControls();
                _playerInputActions.Player.Enable();
            }

            IsPaused = false;
            MovementInput = new Vector2(0, 0);
        }

        private void OnEnable()
        {
            // Move
            _playerInputActions.Player.Move.performed += context => OnMove(context);
            _playerInputActions.Player.Move.canceled += context => OnMove(context);
            // Jump
            _playerInputActions.Player.Jump.started += context => OnJump(context);
            // Run Start
            _playerInputActions.Player.Dash.started += _ => OnDashStart();
            // Run Stop
            _playerInputActions.Player.Dash.canceled += _ => OnDashStop();
            // ADS Start
            _playerInputActions.Player.ADS.performed += _ => OnAdsStart();
            // ADS Stop
            _playerInputActions.Player.ADS.canceled += _ => OnAdsStop();
            // Pause
            _playerInputActions.Player.Pause.performed += _ => OnPause();
            // Character Menu
            _playerInputActions.Player.Inventory.performed += _ => OnInventoryToggled();
        }

        private void OnDisable()
        {
            // Move
            _playerInputActions.Player.Move.performed -= context => OnMove(context);
            _playerInputActions.Player.Move.canceled -= context => OnMove(context);
            // Jump
            _playerInputActions.Player.Jump.started -= context => OnJump(context);
            // Run Start
            _playerInputActions.Player.Dash.started -= _ => OnDashStart();
            // Run Stop
            _playerInputActions.Player.Dash.canceled -= _ => OnDashStop();
            // ADS Start
            _playerInputActions.Player.ADS.performed -= _ => OnAdsStart();
            // ADS Stop
            _playerInputActions.Player.ADS.canceled -= _ => OnAdsStop();
            // Pause
            _playerInputActions.Player.Pause.performed -= _ => OnPause();
            // Character Menu
            _playerInputActions.Player.Inventory.performed -= _ => OnInventoryToggled();

        }

        #endregion

        #region On Performed Methods

        private void OnMove(InputAction.CallbackContext context)
        {
            MovementInput = context.ReadValue<Vector2>();
        }

        private void OnJump(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _jumpPressed = true;
            }        
        }

        private void OnDashStart()
        {
            _playerCoordinator.MovementController.OnDashActionStart();
        }

        private void OnDashStop()
        {
            _playerCoordinator.MovementController.OnDashActionStop();
        }

        private void OnAdsStart()
        {
            _playerCoordinator.MovementController.OnAdsStart();
        }

        private void OnAdsStop()
        {
            _playerCoordinator.MovementController.OnAdsStop();
        }

        private void OnPause()
        {
            _isPaused = !_isPaused;
            if (_isPaused)
            {
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            _playerCoordinator.MovementController.OnGamePaused();
        }

        private void OnInventoryToggled()
        {
            _playerCoordinator.InventoryController.OnInventoryToggled();
        }
        
        #endregion
        
        public bool ConsumeJumpPressed()
        {
            if (_jumpPressed)
            {
                _jumpPressed = false; 
                return true;
            }
            return false;
        }
    }
}