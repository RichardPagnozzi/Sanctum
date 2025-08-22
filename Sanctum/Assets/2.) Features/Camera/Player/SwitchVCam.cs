using UnityEngine;
using Unity.Cinemachine;


public class SwitchVCam : MonoBehaviour
{
    #region Members
    [SerializeField] private int _priorityBoostAmnt = 10;
    [SerializeField] private Canvas _reticleNormalCanvas;
    [SerializeField] private Canvas _reticleAdsCanvas;

    private PlayerControls _playerInputActions;
    private CinemachineVirtualCamera _virtualCamera;
    private CameraManager _cameraManager;
    #endregion

    private void Awake()
    {
        Initialize();   
    }
    #region Monobehaviours
    private void OnEnable()
    {
        _playerInputActions.Player.ADS.performed += _=>StartAds();
        _playerInputActions.Player.ADS.canceled += _ => CancelAds();
    }
    private void OnDisable()
    {
        _playerInputActions.Player.ADS.performed -= _ => StartAds();
        _playerInputActions.Player.ADS.canceled -= _ => CancelAds();
    }
    #endregion

    #region Private Methods
    public void StartAds()
    {
            _virtualCamera.Priority += _priorityBoostAmnt;
            _reticleAdsCanvas.enabled = true;
            _reticleNormalCanvas.enabled = false;
    }
    private void CancelAds()
    {
            _virtualCamera.Priority -= _priorityBoostAmnt;
            _reticleNormalCanvas.enabled = true;
            _reticleAdsCanvas.enabled = false;
    }
    private void Initialize()
    {
        _cameraManager = GameManager.Instance.ServiceLocator.GetService<CameraManager>();
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _playerInputActions = new PlayerControls();
        _playerInputActions.Player.Enable();
    }
    #endregion
}
