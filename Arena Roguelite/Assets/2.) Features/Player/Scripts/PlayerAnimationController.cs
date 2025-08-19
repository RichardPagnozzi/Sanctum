using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class PlayerAnimationController : MonoBehaviour
{

    [SerializeField] private Animator _animator;
    private PlayerControls _playerInputActions;
    private PlayerMovementController _movementController;
    private InputAction _moveAction;

    private void Awake()
    {
        Initialize();
    }
    private void OnEnable()
    {
        _moveAction = _playerInputActions.Player.Move;
        _movementController = GetComponent<PlayerMovementController>();
    }

    private void Update()
    {
        MovementAnimation();
    }

    private void Initialize()
    {
        _playerInputActions = new PlayerControls();
        _playerInputActions.Player.Enable();
    }
    private void MovementAnimation()
    {

        float velocity = _movementController.GetPlayerJumpVelocity();
        Vector2 input = _moveAction.ReadValue<Vector2>();
        _animator.SetBool("Grounded", _movementController.GetGroundedStatus());
        _animator.SetFloat("VelocityX", input.x, 0.1f, Time.deltaTime);
        _animator.SetFloat("VelocityZ", input.y, 0.1f, Time.deltaTime);
        _animator.SetFloat("VerticalSpeed", velocity, 0.1f, Time.deltaTime);
    }
}
