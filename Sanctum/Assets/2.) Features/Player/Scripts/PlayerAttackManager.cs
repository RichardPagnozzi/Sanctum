using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class PlayerAttackManager : MonoBehaviour
{
    #region Members
    private Weapon _equippedWeapon;
    private PlayerControls _playerInputActions;
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
    }
    private void OnDisable()
    {
        _playerInputActions.Player.Attack.started -= _ => Attack();
        _playerInputActions.Player.Attack.canceled -= _ => AttackStop();
        _playerInputActions.Player.Reload.performed -= _ => Reload();
    }
    #endregion


    #region Private
    private void Initialize()
    {
        _playerInputActions = new PlayerControls();
        _playerInputActions.Player.Enable();
    }
    private void Attack()
    {
        if (_equippedWeapon)
        {
            _equippedWeapon?.AttackStart?.Invoke();
        }
    }
    private void AttackStop()
    {
        if (_equippedWeapon)
        {
            _equippedWeapon?.AttackStop?.Invoke();
        }
    }
    private void Reload()
    {
        _equippedWeapon?.ReloadMag();
    }

    #endregion

    #region Public
    public void SetEquippedWeapon(Weapon _toEquip)
    {
        if (_equippedWeapon)
        {
            Destroy(_equippedWeapon.gameObject);
        }
        _equippedWeapon = _toEquip;
        GameManager.Instance.ServiceLocator.EventManager.OnWeaponEquipped.Invoke(_equippedWeapon);
    }
    public Weapon GetEquippedWeapon()
    {
        return _equippedWeapon;
    }
    #endregion 
}
