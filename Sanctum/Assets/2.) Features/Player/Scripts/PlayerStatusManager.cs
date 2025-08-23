using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[System.Serializable]
public class PlayerStatusManager : PlayerBase
{
    #region Members
    public float CurPlayerHealth { get => _curPlayerHealth; private set => _curPlayerHealth = value; }
    public float CurPlayerArmor { get => _curPlayerArmor; private set =>_curPlayerArmor = value; }
    public float CurPlayerEnergy { get => _curPlayerEnergy; private set => _curPlayerEnergy = value; }
    public bool IsInitialized
    {
        get => _isInitialized; 
    }

    private bool _rechargeEnergy = true;
    private float _energyTimer = 0;
    private float _curPlayerHealth;
    private float _curPlayerArmor;
    private float _curPlayerEnergy;
    private bool _isInitialized = false;
    private const float TickIntervalDuration = 0.2f;

    public bool CanRechargeEnergy
    {
        get => _rechargeEnergy; 
        set => _rechargeEnergy = value;
    }
    
    #endregion 

    #region Monobehaviours
    private void Awake()
    {
        if (TryGetPlayerDetails())
        {
            Initialize();
        }
    }
    private void OnEnable()
    {
        GameManager.Instance.ServiceLocator.EventManager.OnPlayerRecieveDamage += RecieveDamage;
        GameManager.Instance.ServiceLocator.EventManager.OnPlayerRecieveTickDamage += RecieveTickDamage;
        GameManager.Instance.ServiceLocator.EventManager.OnPlayerRecieveHealth += RecieveHealth;
        GameManager.Instance.ServiceLocator.EventManager.OnPlayerRecieveTickHealth += RecieveTickHealth;
        GameManager.Instance.ServiceLocator.EventManager.OnPlayerIncreaseMaxHealth += IncreaseMaxHealth;
        GameManager.Instance.ServiceLocator.EventManager.OnPlayerRecieveFlatEnergy += RecieveFlatEnergy;
        GameManager.Instance.ServiceLocator.EventManager.OnPlayerLoseEnergy += LoseEnergy;
        GameManager.Instance.ServiceLocator.EventManager.OnPlayerRecieveTickEnergy += RecieveTickEnergy;
        GameManager.Instance.ServiceLocator.EventManager.OnPlayerIncreaseMaxEnergy += IncreaseMaxEnergy;
        GameManager.Instance.ServiceLocator.EventManager.OnPlayerRecieveArmor += RecieveArmor;
        GameManager.Instance.ServiceLocator.EventManager.OnPlayerIncreaseMaxArmor += IncreaseMaxArmor;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        _isInitialized = true;
    }
    private void OnDisable()
    {
        GameManager.Instance.ServiceLocator.EventManager.OnPlayerRecieveDamage -= RecieveDamage;
        GameManager.Instance.ServiceLocator.EventManager.OnPlayerRecieveTickDamage -= RecieveTickDamage;
        GameManager.Instance.ServiceLocator.EventManager.OnPlayerRecieveHealth -= RecieveHealth;
        GameManager.Instance.ServiceLocator.EventManager.OnPlayerRecieveTickHealth -= RecieveTickHealth;
        GameManager.Instance.ServiceLocator.EventManager.OnPlayerIncreaseMaxHealth -= IncreaseMaxHealth;
        GameManager.Instance.ServiceLocator.EventManager.OnPlayerRecieveFlatEnergy -= RecieveFlatEnergy;
        GameManager.Instance.ServiceLocator.EventManager.OnPlayerLoseEnergy -= LoseEnergy;
        GameManager.Instance.ServiceLocator.EventManager.OnPlayerRecieveTickEnergy -= RecieveTickEnergy;
        GameManager.Instance.ServiceLocator.EventManager.OnPlayerIncreaseMaxEnergy -= IncreaseMaxEnergy;
        GameManager.Instance.ServiceLocator.EventManager.OnPlayerRecieveArmor -= RecieveArmor;
        GameManager.Instance.ServiceLocator.EventManager.OnPlayerIncreaseMaxArmor -= IncreaseMaxArmor;
    }
    private void Update()
    {
        RechargeEnergy();
    }

    private void Initialize()
    {
        _curPlayerHealth = CurrentPlayerDetails.Stats.Health;
        _curPlayerArmor = CurrentPlayerDetails.Stats.Armor;
        _curPlayerEnergy = CurrentPlayerDetails.Stats.Energy;
    }
    #endregion
    
    #region Private
    // Threshold Checks
    private void PlayerDeathCheck()
    {
        if (_curPlayerHealth <= 0)
        {
            GameManager.Instance.ServiceLocator.EventManager.OnPlayerIsDead.Invoke();
        }
    }
    private void PlayerNegativeHealthCheck()
    {
        if (_curPlayerHealth <= 0)
        {
            _curPlayerHealth = 0;
        }
    }
    private void PlayerMaxHealthCheck()
    {
        if (_curPlayerHealth >= CurrentPlayerDetails.Stats.Health)
        {
            _curPlayerHealth = CurrentPlayerDetails.Stats.Health;
        }
    }
    private void PlayerMaxEnergyCheck()
    {
        if (_curPlayerEnergy >= CurrentPlayerDetails.Stats.Energy)
        {
            _curPlayerEnergy = CurrentPlayerDetails.Stats.Energy;
        }
    }
    private void PlayerMaxArmorCheck()
    {
        if (_curPlayerArmor >= CurrentPlayerDetails.Stats.Armor)
        {
            _curPlayerArmor = CurrentPlayerDetails.Stats.Armor;
        }
    }
    // Damage
    private void RecieveDamage(float dmg)
    {
        float rawDamageAmnt = dmg;
        _curPlayerHealth -= rawDamageAmnt;
        PlayerNegativeHealthCheck();
        GameManager.Instance.ServiceLocator.EventManager.OnPlayerHealthModified.Invoke();
        PlayerDeathCheck();
    }
    private void RecieveTickDamage(float dmg, int intervals)
    {
        StartCoroutine(TickDamage(dmg, intervals));
    }
    private IEnumerator TickDamage(float dmg, int intervals)
    {
        float curInterval = 0;
        while (curInterval < intervals)
        {
            RecieveDamage(dmg);
            yield return new WaitForSeconds(TickIntervalDuration);
            curInterval++;
        }
    }
    // Health
    private void RecieveHealth(float health)
    {
        if (_curPlayerHealth == CurrentPlayerDetails.Stats.Health)
        {
            return;
        }
        _curPlayerHealth += health;
        PlayerMaxHealthCheck();
        GameManager.Instance.ServiceLocator.EventManager.OnPlayerHealthModified.Invoke();
    }
    private void RecieveTickHealth(float health, int intervals)
    {
        StartCoroutine(TickHealth(health, intervals));
    }
    private IEnumerator TickHealth(float health, int intervals)
    {
        float curInterval = 0;
        while (curInterval < intervals)
        {
            RecieveHealth(health);
            yield return new WaitForSeconds(TickIntervalDuration);
            curInterval++;
        }
    }
    private void IncreaseMaxHealth(float health)
    {
        float newMaxHealthValue = (CurrentPlayerDetails.Stats.Health) + health;
        CurrentPlayerDetails.Stats.SetPlayerStat(PlayerStats.StatName.Health, newMaxHealthValue);
        GameManager.Instance.ServiceLocator.EventManager.OnPlayerHealthModified.Invoke();
    }
    // Armor
    private void RecieveArmor(float armor)
    {
        _curPlayerArmor += armor;
        PlayerMaxArmorCheck();
        GameManager.Instance.ServiceLocator.EventManager.OnPlayerArmorModified.Invoke();
    }
    private void IncreaseMaxArmor(float armor)
    {
        float newMaxArmorValue = (CurrentPlayerDetails.Stats.Armor) + armor;
        CurrentPlayerDetails.Stats.SetPlayerStat(PlayerStats.StatName.Armor, newMaxArmorValue);
        GameManager.Instance.ServiceLocator.EventManager.OnPlayerArmorModified.Invoke();
    }
    // Energy 
    private void RecieveFlatEnergy(float energy)
    {
        if (_curPlayerEnergy == CurrentPlayerDetails.Stats.Energy)
        {
            return;
        }
        _curPlayerEnergy += energy;
        PlayerMaxEnergyCheck();
        GameManager.Instance.ServiceLocator.EventManager.OnPlayerEnergyModified.Invoke();
    }
    
    private void LoseEnergy(float energy)
    {
        _curPlayerEnergy -= energy;
        if(_curPlayerEnergy < 0)
        {
            _curPlayerEnergy = 0;
        }
        GameManager.Instance.ServiceLocator?.EventManager?.OnPlayerEnergyModified?.Invoke();
    }
    public bool AffordEnergyCost(float energy)
    {
        bool canAfford = (_curPlayerEnergy - energy < 0) ? false : true;
        return canAfford;
    }
    private void RecieveTickEnergy(float energy, int intervals)
    {
        StartCoroutine(TickEnergy(energy, intervals));
    }
    private IEnumerator TickEnergy(float energy, int intervals)
    {
        float curInterval = 0;
        while (curInterval < intervals)
        {
            RecieveFlatEnergy(energy);
            yield return new WaitForSeconds(TickIntervalDuration);
            curInterval++;
        }
    }
    private void IncreaseMaxEnergy(float energy)
    {
        float newMaxEnergyValue = (CurrentPlayerDetails.Stats.Energy) + energy;
        CurrentPlayerDetails.Stats.SetPlayerStat(PlayerStats.StatName.Energy, newMaxEnergyValue);
        GameManager.Instance.ServiceLocator.EventManager.OnPlayerEnergyModified.Invoke();
    }
    private void RechargeEnergy()
    {
        if (_rechargeEnergy)
        {
            if (_curPlayerEnergy == CurrentPlayerDetails.Stats.Energy)
            {
                return;
            }

            _energyTimer += Time.deltaTime;
            if (_energyTimer >= TickIntervalDuration)
            {
                _curPlayerEnergy += CurrentPlayerDetails.Stats.EnergyRechargeAmmount;
                PlayerMaxEnergyCheck();
                GameManager.Instance.ServiceLocator?.EventManager?.OnPlayerEnergyModified?.Invoke();
                _energyTimer = 0;
            }
        }
    }
    #endregion 

}
