using System;
using UnityEngine;

namespace Player
{
    [Serializable]
    public class PlayerAnimationController : PlayerBase
    {
        [SerializeField] private Animator _animator;
        private PlayerCoordinator _playerCoordinator;
        private Vector2 _movementInput;
        private const string AnimValueGrounded = "Grounded";
        private const string AnimValueVelocityX = "VelocityX";
        private const string AnimValueVelocityZ = "VelocityZ";
        private const string AnimValueVertSpeed = "VerticalSpeed";
        
        public Vector2 MovementInput
        {
            get => _movementInput;
            set { _movementInput = value; }
        }

        private void Awake()
        {
            Initialize();
        }

        private void Update()
        {
            PerformMovementAnimation(_playerCoordinator.InputManager.MovementInput);
        }

        private void Initialize()
        {
            _playerCoordinator = GetComponent<PlayerCoordinator>();
        }

        private void MovementAnimation(Vector2 input)
        {
            float velocity = _playerCoordinator.MovementController.GetPlayerJumpVelocity();
            _animator.SetBool(AnimValueGrounded, _playerCoordinator.MovementController.GetGroundedStatus());
            _animator.SetFloat(AnimValueVelocityX, input.x, 0.1f, Time.deltaTime);
            _animator.SetFloat(AnimValueVelocityZ, input.y, 0.1f, Time.deltaTime);
            _animator.SetFloat(AnimValueVertSpeed, velocity, 0.1f, Time.deltaTime);
        }

        private void PerformMovementAnimation(Vector2 input)
        {
            MovementAnimation(input);
        }
        
    }
}