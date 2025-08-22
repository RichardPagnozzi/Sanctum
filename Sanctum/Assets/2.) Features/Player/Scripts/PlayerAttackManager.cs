using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class PlayerAttackManager : MonoBehaviour
{
    #region Members
    private Weapon _equippedWeapon;
    private PlayerControls _playerInputActions;
    private CameraManager _cameraManager;
    private Coroutine _reloadRoutine;
    private bool _isReloading;

    
    #endregion

    #region Monobehaviours
    private void Awake()
    {
        Initialize();
    }
    private void OnEnable()
    {
        _playerInputActions.Player.Attack.started += _ => Attack();
        _playerInputActions.Player.Attack.canceled += _ => AttackStop();
        _playerInputActions.Player.Reload.performed += _ => Reload();
        _playerInputActions.Player.ADS.performed += _=> StartADS();
        _playerInputActions.Player.ADS.canceled += _ => CancelADS();
    }
    private void OnDisable()
    {
        _playerInputActions.Player.Attack.started -= _ => Attack();
        _playerInputActions.Player.Attack.canceled -= _ => AttackStop();
        _playerInputActions.Player.Reload.performed -= _ => Reload();
        _playerInputActions.Player.ADS.performed -= _ => StartADS();
        _playerInputActions.Player.ADS.canceled -= _ => CancelADS();
    }
    #endregion
    
    #region Private
    private void Initialize()
    {
        _playerInputActions = new PlayerControls();
        _playerInputActions.Player.Enable();
        _cameraManager = GameManager.Instance.ServiceLocator.GetService<CameraManager>();
    }
    
    private void Attack()
    {
        if(!_isReloading)
        _equippedWeapon?.AttackStart?.Invoke();
    }
    
    private void AttackStop()
    {
        _equippedWeapon?.AttackStop?.Invoke();
    }
    
    private void Reload()
    {
        if (_reloadRoutine != null) return; 
        float reloadTime = (float)_equippedWeapon?.WeaponDetail.reloadSpeed;
        _reloadRoutine = StartCoroutine(ReloadRoutine(reloadTime));
    }

    private IEnumerator ReloadRoutine(float reloadTime)
    {
        _isReloading = true;
        GameManager.Instance.ServiceLocator.EventManager.OnWeaponReloadStart(reloadTime);
        yield return new WaitForSeconds(reloadTime);
        _equippedWeapon?.ReloadMag();
        GameManager.Instance.ServiceLocator.EventManager.OnWeaponReloaded();
        _isReloading = false;
        _reloadRoutine = null;
    }
    
    private void StartADS()
    {
        _cameraManager?.SwitchToCamera(KeywordDictionary.Cameras.vcam_ADS.ToString());
    }

    private void CancelADS()
    {
        _cameraManager?.SwitchToCamera(KeywordDictionary.Cameras.vcam_Normal.ToString());
    }
    
    #endregion

    #region Public
    // Sets the weapon the attack manager uses to call attack methods on
    public void SetEquippedWeapon(Weapon _toEquip)
    {
        if (_equippedWeapon)
        {
            Destroy(_equippedWeapon.gameObject);
        }
        _equippedWeapon = _toEquip;
        GameManager.Instance.ServiceLocator.EventManager.OnWeaponEquipped.Invoke(_equippedWeapon);
    }
   
    // Returns the currently equipped weapon
    public Weapon GetEquippedWeapon()
    {
        return _equippedWeapon;
    }
    #endregion 
}
