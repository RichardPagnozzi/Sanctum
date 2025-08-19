using System;
using System.Collections;
using UnityEngine;


public class PlayerMovementController : PlayerBase
{
    #region Members
    // Serialized
    [SerializeField] private GameObject _trailRenderer;
    [SerializeField] private GameObject _aimAtObject;
    // Aim Values
    private Vector3 _aimAtPosition;
    private float _nextJumpInterval = 0;
    private int _curJumpCount = 0;
    private float _time;
    private bool _canJump = true;
    private Vector3 _playerVelocity;
    private Vector3 _moveDirection = default;
    private bool _groundedPlayer;
    private bool _isMoving;
    private bool _isDashing;
    private bool _isADS;
    private bool _isPaused = false;
    // References
    private CharacterController _controller;
    private PlayerStatusManager _playerStatusManager;
    private Transform _cameraTransform;
    // Input
    private PlayerControls _playerInputActions;
    // Consts
    private const string AnimMoveBool = "isWalking";
    private const int _aimRange = 50;
    private const int _layerMask = ~(1 << 2);
    private const float _rotationSpeed = 20f;
    #endregion

    #region Monobehaviours
    private void Awake()
    {
        if (TryGetPlayerDetails())
        {
            Initialize();
        }
        else
        {
            enabled = false;
        }
    }
    private void OnEnable()
    {
        _playerInputActions.Player.Dash.started += _=>DashActionStart();
        _playerInputActions.Player.Dash.canceled +=_=> DashActionStop();
        _playerInputActions.Player.ADS.performed += _ => SnapTowardCameraDirection();
        _playerInputActions.Player.ADS.canceled += _ => CancelADS();
        _playerInputActions.Player.Pause.performed += _ => Pause();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void OnDisable()
    {
        _playerInputActions.Player.Dash.started -= _ => DashActionStart();
        _playerInputActions.Player.Dash.canceled -= _ => DashActionStop();
        _playerInputActions.Player.ADS.performed -= _ => SnapTowardCameraDirection();
        _playerInputActions.Player.ADS.canceled -= _ => CancelADS();
        _playerInputActions.Player.Pause.performed -= _ => Pause();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    private void Update()
    {
        PositionAimObject();
        SetGroundedStatus();
        GravityHandler();
        MoveActionHandler();
        JumpActionHandler();
        AimActionHandler();
    }
    #endregion

    #region Private Methods
    // Init
    private void Initialize()
    {
        _playerInputActions = new PlayerControls();
        _playerInputActions.Player.Enable();
        _controller = GetComponent<CharacterController>();
        _playerStatusManager = GetComponent<PlayerStatusManager>();
        _cameraTransform = Camera.main.transform;
    }
    // Movement Actions
    public void TriggerJumpPadAction(float jumpPadHeight)
    {
        _playerVelocity = Vector3.zero;
        _playerVelocity.y += Mathf.Sqrt(jumpPadHeight * -3.0f * CurrentPlayerDetails.Stats.GravityValue);
        _controller.Move(_playerVelocity * Time.deltaTime);
    }
    public float GetPlayerJumpVelocity()
    {
        return _playerVelocity.y;
    }
    public bool GetGroundedStatus()
    {
        return _groundedPlayer;
    }
    private void MoveActionHandler()
    {
        float movementSpeed;
        Vector2 input = _playerInputActions.Player.Move.ReadValue<Vector2>(); // get direction from player input
        _moveDirection = new Vector3(input.x, 0, input.y); // assign direction to Vector3
        _moveDirection = _moveDirection.x * _cameraTransform.right.normalized + _moveDirection.z * _cameraTransform.forward.normalized; // normalize direction with camera 
        if (_isDashing && _playerStatusManager.AffordEnergyCost(CurrentPlayerDetails.Stats.SprintCost)) // assign player speed based on player input    
        {
            movementSpeed = CurrentPlayerDetails.Stats.SprintSpeed;
            GameManager.Instance.ServiceLocator.EventManager.OnPlayerLoseEnergy(CurrentPlayerDetails.Stats.SprintCost);
        }
        else
        {
            movementSpeed = CurrentPlayerDetails.Stats.MovementSpeed;
            DashActionStop();
        }
        _moveDirection.y = 0f;
        _controller.Move(_moveDirection * (Time.deltaTime * movementSpeed)); // move player
        _isMoving = (_moveDirection != Vector3.zero) ? true : false; // toggle isMoving flag 
    }
    private void JumpActionHandler()
    {
        try
        {
            _time = Time.time;
            bool canAffordJump = GetComponent<PlayerStatusManager>().AffordEnergyCost(CurrentPlayerDetails.Stats.JumpCost);
            bool jumpDelayMet = _time >= _nextJumpInterval;
            bool jumpCountMet = _curJumpCount < CurrentPlayerDetails.Stats.Jumps;

            if (_canJump)
            {
                if (canAffordJump && jumpDelayMet && jumpCountMet && _playerInputActions.Player.Jump.triggered)
                {
                    JumpPlayer();
                    _nextJumpInterval = _time + CurrentPlayerDetails.Stats.JumpDelay;
                    _curJumpCount++;

                    if (_curJumpCount >= CurrentPlayerDetails.Stats.Jumps)
                    {
                        _canJump = false;
                        StartCoroutine(JumpResetDelayRoutine());
                    }
                }
            }
            _controller.Move(_playerVelocity * Time.deltaTime);
        }
        catch (Exception e)
        {
            Debug.LogError("EXCEPTION CAUGHT !!!! : " + e.Message);
        }
    }
    private void JumpPlayer()
    {
        _playerVelocity = Vector3.zero;
        _playerVelocity.y += Mathf.Sqrt(CurrentPlayerDetails.Stats.JumpHeight * -3.0f * CurrentPlayerDetails.Stats.GravityValue);
        GameManager.Instance.ServiceLocator.EventManager.OnPlayerLoseEnergy(CurrentPlayerDetails.Stats.JumpCost);
    }
    private IEnumerator JumpResetDelayRoutine()
    {
        yield return new WaitForSeconds(CurrentPlayerDetails.Stats.JumpResetDelay);
        _curJumpCount = 0;
        _canJump = true;
    }
    private void DashActionStart()
    {
        if (_playerStatusManager.AffordEnergyCost(CurrentPlayerDetails.Stats.SprintCost))
        {
            _isDashing = true;
            _trailRenderer.SetActive(true);
        }
    }
    private void DashActionStop()
    {
        _isDashing = false;
        _trailRenderer.SetActive(false);
    }
    private void PositionAimObject()
    {
        _aimAtPosition = _aimAtObject.transform.position;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, _aimRange, _layerMask))
        {
            _aimAtObject.transform.position = hit.point;
        }
        else
        {
            _aimAtObject.transform.position = ray.GetPoint(_aimRange);
        }
    }
    private void AimActionHandler()
    {
        // Rotates player toward camera direction
        Quaternion targetRotation = Quaternion.Euler(0, _cameraTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }
    private void SnapTowardCameraDirection()
    {
        _isADS = true;
        Quaternion targetRotation = Quaternion.Euler(0, _cameraTransform.eulerAngles.y, 0);
        transform.rotation = targetRotation;
    }
    // Helper
    private void SetGroundedStatus()
    {
        _groundedPlayer = _controller.isGrounded;
    }
    private void GravityHandler()
    {
        if (!_groundedPlayer)
            _playerVelocity.y += CurrentPlayerDetails.Stats.GravityValue * Time.deltaTime * 2;
        else
            _playerVelocity.y = CurrentPlayerDetails.Stats.GravityValue;
    }
    private void CancelADS()
    {
        _isADS = false;
    }
    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Pause()
    {
        _isPaused = !_isPaused;
        if (_isPaused)
        {
            Time.timeScale = 0;
            UnlockCursor();
        }
        else
        {
            Time.timeScale = 1;
            LockCursor();
        }
    }
    #endregion 
}
