using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;

public class uiHudController : MonoBehaviour
{
    #region Members

    [SerializeField] PlayerStatusManager _playerStatusManager;
    [Header("Vitals")] 
    [SerializeField] private RectTransform _healthArmorBackground;
    [SerializeField] private Image _healthSlider;
    [SerializeField] private Image _energySlider;
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private TMP_Text _energyText;
    [Header("Attack")] [SerializeField] private TextMeshProUGUI _weaponNameLabel;
    [SerializeField] private TextMeshProUGUI _weaponAmmoLabel;
    private Vector2 _originalHealthArmorBGPosition;
    private Weapon _equippedWeaponReference;
    private HorizontalLayoutGroup _layoutGroup;
    #endregion

    #region Monobehaviours

    private void OnEnable()
    {
        SetPlayerReferencesRoutine();
        GameManager.Instance.ServiceLocator.EventManager.OnPlayerRecieveDamage += ShakeBackground;
        GameManager.Instance.ServiceLocator.EventManager.OnPlayerHealthModified += UpdateHealthBarFill;
        GameManager.Instance.ServiceLocator.EventManager.OnPlayerEnergyModified += UpdateEnergyBarFill;
        GameManager.Instance.ServiceLocator.EventManager.OnWeaponEquipped += UpdateEquippedWeaponNameLabel;
        GameManager.Instance.ServiceLocator.EventManager.OnWeaponFired += UpdateEquippedWeaponAmmoLabel;
        GameManager.Instance.ServiceLocator.EventManager.OnWeaponReloaded += UpdateEquippedWeaponAmmoLabel;
    }

    private void OnDisable()
    {
        GameManager.Instance.ServiceLocator.EventManager.OnPlayerRecieveDamage -= ShakeBackground;
        GameManager.Instance.ServiceLocator.EventManager.OnPlayerHealthModified -= UpdateHealthBarFill;
        GameManager.Instance.ServiceLocator.EventManager.OnPlayerEnergyModified -= UpdateEnergyBarFill;
        GameManager.Instance.ServiceLocator.EventManager.OnWeaponEquipped -= UpdateEquippedWeaponNameLabel;
        GameManager.Instance.ServiceLocator.EventManager.OnWeaponFired -= UpdateEquippedWeaponAmmoLabel;
        GameManager.Instance.ServiceLocator.EventManager.OnWeaponReloaded -= UpdateEquippedWeaponAmmoLabel;
    }

    private void Start()
    {
        _originalHealthArmorBGPosition = _healthArmorBackground.position;
        _layoutGroup = GetComponentInParent<HorizontalLayoutGroup>();
    }

    #endregion

    #region Private
    
    private void UpdateHealthBarFill()
    {
        if (_playerStatusManager != null)
        {
            float targetFillAmount = _playerStatusManager.CurPlayerHealth / GameManager.Instance.PlayerRepository.CurrentSessionPlayerDetails.Stats.Health;
            StartCoroutine(SmoothHealthSliderUpdate(_healthSlider, targetFillAmount, _healthText));
        }
    }
    
    private void UpdateEnergyBarFill()
    {
        if (_playerStatusManager != null)
        {
            
            _energySlider.fillAmount = _playerStatusManager.CurPlayerEnergy / GameManager.Instance.PlayerRepository.CurrentSessionPlayerDetails.Stats.Energy;
            _energyText.text = $"{_playerStatusManager.CurPlayerEnergy.ToString("0")} / {GameManager.Instance.PlayerRepository.CurrentSessionPlayerDetails.Stats.Energy.ToString("0")}";
        }
    }

    private void UpdateEquippedWeaponNameLabel(Weapon reference)
    {
        if (reference != null)
        {
            _equippedWeaponReference = reference;
            _weaponNameLabel.text = reference.WeaponDetail.name.ToString();
        }
        else
        {
            _weaponNameLabel.text = "null";
        }
    }

    private void UpdateEquippedWeaponAmmoLabel()
    {
        if (_equippedWeaponReference != null)
        {
            _weaponAmmoLabel.text = _equippedWeaponReference.ammoInMag.ToString() + "/" +
                                    _equippedWeaponReference.curAmmo.ToString();
        }
        else
        {
            _weaponAmmoLabel.text = "null";
        }
    }

    private void SetPlayerReferencesRoutine()
    {
        StartCoroutine(WaitForPlayerInit());
    }

    private void ShakeBackground(float recieveDmgAmnt = 0)
    {
        _healthArmorBackground.DOKill(complete: true);
        _healthArmorBackground.DOPunchScale(new Vector3(0.075f,0.2f,0), 0.1f, 2, 1);
    }
    
    private IEnumerator WaitForPlayerInit()
    {
        _playerStatusManager = GameManager.Instance.ServiceLocator.GetService<PlayerStatusManager>();
        while (_playerStatusManager.IsInitialized == false)
        {
            yield return null;
        }
        UpdateEquippedWeaponNameLabel(_playerStatusManager.gameObject.GetComponent<PlayerAttackManager>().GetEquippedWeapon());
        UpdateHealthBarFill();
        UpdateEnergyBarFill();
    }

    private IEnumerator SmoothHealthSliderUpdate(Image slider, float targetFillAmount, TMP_Text textToUpdate)
    {
        float currentFillAmount = slider.fillAmount;
        float elapsedTime = 0f;
        float duration = 0.5f; // Adjust the duration to control the speed of the transition

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            slider.fillAmount = Mathf.Lerp(currentFillAmount, targetFillAmount, elapsedTime / duration);
            yield return null;
        }

        slider.fillAmount = targetFillAmount;
        _healthText.text = $"{_playerStatusManager.CurPlayerHealth.ToString("0")} / {GameManager.Instance.PlayerRepository.CurrentSessionPlayerDetails.Stats.Health.ToString("0")}";
    }

    #endregion
}