using System;
using System.Collections;
using Player;
using UnityEngine;

namespace Player
{
    public class PlayerMovementController : PlayerBase
    {
        #region Members

        // Serialized
        [SerializeField] private GameObject _trailRenderer;

        [SerializeField] private GameObject _aimAtObject;
        [SerializeField] private GameObject _ikAimAtObject;

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
        private PlayerCoordinator _playerCoordinator;
        private Transform _cameraTransform;

        // Consts
        private const string AnimMoveBool = "isWalking";
        private const int _aimRange = 50;
        private const int _layerMask = ~(1 << 2);
        private const float _rotationSpeed = 20f;

        #endregion

        #region Lifecycle

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

        private void Update()
        {
            if (!_isPaused && _playerCoordinator is not null)
            {
                PositionAimObject();
                SetGroundedStatus();
                GravityHandler();
                MoveActionHandler();
                JumpActionHandler();
                AimActionHandler();
            }
        }

        #endregion

        #region Public Methods

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

        public bool GetRechargeApproval()
        {
            return _isDashing == false;
        }

        public void OnDashActionStart()
        {
            SprintActionStart();
        }

        public void OnDashActionStop()
        {
            SprintActionStop();
        }

        public void OnJumpAction()
        {
            JumpActionHandler();
        }

        public void OnAdsStart()
        {
            SnapTowardCameraDirection();
        }

        public void OnAdsStop()
        {
            CancelADS();
        }

        public void OnGamePaused()
        {
            _isPaused = _playerCoordinator.InputManager.IsPaused;
            if (_isPaused)
            {
                _ikAimAtObject.SetActive(false);
                _aimAtObject.SetActive(false);
            }
            else
            {
                _ikAimAtObject.SetActive(true);
                _aimAtObject.SetActive(true);
            }
        }

        #endregion

        #region Private Methods

        private void Initialize()
        {
            _controller = GetComponent<CharacterController>();
            _playerCoordinator = GetComponent<PlayerCoordinator>();
            _cameraTransform = Camera.main.transform;
        }

        private void MoveActionHandler()
        {
            float movementSpeed;
            Vector2 input = _playerCoordinator.InputManager.MovementInput;
            _moveDirection = new Vector3(input.x, 0, input.y); // assign direction to Vector3
            _moveDirection = _moveDirection.x * _cameraTransform.right.normalized +
                             _moveDirection.z * _cameraTransform.forward.normalized; // normalize direction with camera 
            if (_isDashing &&
                _playerCoordinator.StatusController.AffordEnergyCost(CurrentPlayerDetails.Stats
                    .SprintCost)) // assign player speed based on player input    
            {
                movementSpeed = CurrentPlayerDetails.Stats.SprintSpeed;
                GameManager.Instance.ServiceLocator.EventManager.OnPlayerLoseEnergy(CurrentPlayerDetails.Stats
                    .SprintCost);
            }
            else
            {
                movementSpeed = CurrentPlayerDetails.Stats.MovementSpeed;
                SprintActionStop();
            }

            _moveDirection.y = 0f;
            _controller.Move(_moveDirection * (Time.deltaTime * movementSpeed)); // move player
            _isMoving = (_moveDirection != Vector3.zero); // toggle isMoving flag 
        }

        private void JumpActionHandler()
        {
            try
            {
                _time = Time.time;
                bool canAffordJump =
                    _playerCoordinator.StatusController.AffordEnergyCost(CurrentPlayerDetails.Stats.JumpCost);
                bool jumpDelayMet = _time >= _nextJumpInterval;
                bool jumpCountMet = _curJumpCount < CurrentPlayerDetails.Stats.MaxJumps;

                if (_playerCoordinator.InputManager.ConsumeJumpPressed())
                {
                    if (_canJump && canAffordJump && jumpDelayMet && jumpCountMet)
                    {
                        JumpPlayer();
                        _nextJumpInterval = _time + CurrentPlayerDetails.Stats.JumpDelay;
                        _curJumpCount++;

                        if (_curJumpCount >= CurrentPlayerDetails.Stats.MaxJumps)
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
                Debug.LogError("Exception while jumping: " + e.Message);
            }
        }


        private void JumpPlayer()
        {
            _playerVelocity = Vector3.zero;
            _playerVelocity.y +=
                Mathf.Sqrt(CurrentPlayerDetails.Stats.JumpHeight * -3.0f * CurrentPlayerDetails.Stats.GravityValue);
            GameManager.Instance.ServiceLocator.EventManager.OnPlayerLoseEnergy(CurrentPlayerDetails.Stats.JumpCost);
        }

        private IEnumerator JumpResetDelayRoutine()
        {
            yield return new WaitForSeconds(CurrentPlayerDetails.Stats.JumpResetDelay);
            _curJumpCount = 0;
            _canJump = true;
        }

        private void SprintActionStart()
        {
            if (_playerCoordinator.StatusController.AffordEnergyCost(CurrentPlayerDetails.Stats.SprintCost))
            {
                _isDashing = true;
                _trailRenderer.SetActive(true);
                _playerCoordinator.StatusController.CanRechargeEnergy = false;
            }
        }

        private void SprintActionStop()
        {
            _isDashing = false;
            _trailRenderer.SetActive(false);
            _playerCoordinator.StatusController.CanRechargeEnergy = true;
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

        private void SetGroundedStatus()
        {
            _groundedPlayer = _controller.isGrounded;
        }

        private void GravityHandler()
        {
            if (!_groundedPlayer)
            {
                _playerVelocity.y += CurrentPlayerDetails.Stats.GravityValue * Time.deltaTime * 2;
            }
            else
            {
                _playerVelocity.y = CurrentPlayerDetails.Stats.GravityValue;
            }
        }


        private void CancelADS()
        {
            _isADS = false;
        }

        #endregion
    }
}