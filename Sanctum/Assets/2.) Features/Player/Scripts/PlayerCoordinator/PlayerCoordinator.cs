using System;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerMovementController))]
    [RequireComponent(typeof(PlayerAnimationController))]
    [RequireComponent(typeof(PlayerAttackManager))]
    [RequireComponent(typeof(PlayerInventoryManager))]
    [RequireComponent(typeof(PlayerStatusManager))]
    [RequireComponent(typeof(PlayerInputManager))]
    public class PlayerCoordinator : MonoBehaviour
    {
        private PlayerMovementController _playerMovementController;
        private PlayerAnimationController _playerAnimationController;
        private PlayerAttackManager _playerAttackManager;
        private PlayerInventoryManager _playerInventoryManager;
        private PlayerStatusManager _playerStatusManager;
        private PlayerInputManager _playerInputManager;


        public PlayerMovementController MovementController
        {
            get => _playerMovementController;
            private set { _playerMovementController = value; }
        }

        public PlayerAnimationController AnimationController
        {
            get => _playerAnimationController;
            private set { _playerAnimationController = value; }
        }

        public PlayerAttackManager AttackController
        {
            get => _playerAttackManager;
            private set { _playerAttackManager = value; }
        }

        public PlayerInventoryManager InventoryController
        {
            get => _playerInventoryManager;
            private set { _playerInventoryManager = value; }
        }

        public PlayerStatusManager StatusController
        {
            get => _playerStatusManager;
            private set { _playerStatusManager = value; }
        }

        public PlayerInputManager InputManager
        {
            get => _playerInputManager;
            private set { _playerInputManager = value; }
        }


        private void Awake()
        {
            try
            {
                MovementController = _playerMovementController = GetComponent<PlayerMovementController>();
                AnimationController = _playerAnimationController = GetComponent<PlayerAnimationController>();
                AttackController = _playerAttackManager = GetComponent<PlayerAttackManager>();
                InventoryController = _playerInventoryManager = GetComponent<PlayerInventoryManager>();
                StatusController = _playerStatusManager = GetComponent<PlayerStatusManager>();
                InputManager = _playerInputManager = GetComponent<PlayerInputManager>();
            }
            catch(Exception e ){DebugLogger.Log("Exception Caught in Coordinator: " + e.Message);}
        }
    }
}