using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class PlayerStats
{
    [NonSerialized] private Dictionary<StatName, Action<float>> _statSetters;

    public enum StatName
    {
        Level,
        Health,
        Energy,
        EnergyRechargeAmmount,
        Armor,
        MovementSpeed,
        MovementRotationSpeed,
        SprintSpeed,
        SprintCost,
        GravityValue,
        AimAtObjectRange,
        JumpHeight,
        JumpDelay,
        JumpResetDelay,
        JumpCost,
        Jumps,
        AttackDamageModifier,
        AttackVelocityModifier,
        CriticalChanceModifier,
        CriticalDamageModifier,
        ArmorPiercingModifier,
        AmmoPoolModifier,
        InventorySlots,
        ActiveItemSlots,
        StartingInventorySlots,
        StartingActiveItemSlots
    }

    #region Serialization

    // Gameplay
    [SerializeField] private float _level;
    
    // Vitals
    [SerializeField] private float _health;
    [SerializeField] private float _energy;
    [SerializeField] private float _energyRechargeAmmount;
    [SerializeField] private float _armor;
    
    // Movement modifiers
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _movementRotationSpeed;
    [SerializeField] private float _sprintSpeed;
    [SerializeField] private float _sprintCost;
    [SerializeField] private float _gravityValue;
    [SerializeField] private float _aimAtObjectRange;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _jumpDelay;
    [SerializeField] private float _jumpResetDelay;
    [SerializeField] private float _jumpCost;

    [SerializeField] private int _maxJumps;

    // Damage modifiers
    [SerializeField] private float _attackDamageModifier;
    [SerializeField] private float _attackVelocityModifier;
    [SerializeField] private float _criticalChanceModifier;
    [SerializeField] private float _criticalDamageModifier;
    [SerializeField] private float _armorPiercingModifier;

    [SerializeField] private int _ammoPoolModifier;

    // Items
    [SerializeField] private int _inventorySlots;
    [SerializeField] private int _activeItemSlots;
    [SerializeField] private int _startingInventorySlots;
    [SerializeField] private int _startingActiveItemSlots;

    #endregion

    #region Readonly Accessors

    // Gameplay
    public float Level => _level;
    // Vitals
    public float Health => _health;
    public float Energy => _energy;
    public float EnergyRechargeAmmount => _energyRechargeAmmount;
    public float Armor => _armor;

    // Movement modifiers
    public float MovementSpeed => _movementSpeed;
    public float MovementRotationSpeed => _movementRotationSpeed;
    public float SprintSpeed => _sprintSpeed;
    public float SprintCost => _sprintCost;
    public float GravityValue => _gravityValue;
    public float AimAtObjectRange => _aimAtObjectRange;
    public float JumpHeight => _jumpHeight;
    public float JumpDelay => _jumpDelay;
    public float JumpResetDelay => _jumpResetDelay;
    public float JumpCost => _jumpCost;
    public int MaxJumps => _maxJumps;

    // Damage modifiers
    public float AttackDamageModifier => _attackDamageModifier;
    public float AttackVelocityModifier => _attackVelocityModifier;
    public float CriticalChanceModifier => _criticalChanceModifier;
    public float CriticalDamageModifier => _criticalDamageModifier;
    public float ArmorPiercingModifier => _armorPiercingModifier;
    public int AmmoPoolModifier => _ammoPoolModifier;

    // Items
    public int InventorySlots => _inventorySlots;
    public int ActiveItemSlots => _activeItemSlots;
    public int StartingInventorySlots => _startingInventorySlots;
    public int StartingActiveItemSlots => _startingActiveItemSlots;

    #endregion

    public PlayerStats(float level,
        float health, float energy, float energyRechargeAmmount, float armor, float movementSpeed,
        float movementRotationSpeed, float sprintSpeed, float sprintCost, float gravityValue, float aimAtObjectRange,
        float jumpHeight, float jumpDelay, float jumpResetDelay, float jumpCost, int maxJumps, float attackDamageModifier,
        float attackVelocityModifier, float criticalChanceModifier, float criticalDamageModifier,
        float armorPiercingModifier,
        int ammoPoolModifier, int inventorySlots, int activeItemSlots, int startingInventorySlots,
        int startingActiveItemSlots)
    {
        _level = level;
        _health = health;
        _energy = energy;
        _energyRechargeAmmount = energyRechargeAmmount;
        _armor = armor;
        _movementSpeed = movementSpeed;
        _movementRotationSpeed = movementRotationSpeed;
        _sprintSpeed = sprintSpeed;
        _sprintCost = sprintCost;
        _gravityValue = gravityValue;
        _aimAtObjectRange = aimAtObjectRange;
        _jumpHeight = jumpHeight;
        _jumpDelay = jumpDelay;
        _jumpResetDelay = jumpResetDelay;
        _jumpCost = jumpCost;
        _maxJumps = maxJumps;
        _attackDamageModifier = attackDamageModifier;
        _attackVelocityModifier = attackVelocityModifier;
        _criticalChanceModifier = criticalChanceModifier;
        _criticalDamageModifier = criticalDamageModifier;
        _armorPiercingModifier = armorPiercingModifier;
        _ammoPoolModifier = ammoPoolModifier;
        _inventorySlots = inventorySlots;
        _activeItemSlots = activeItemSlots;
        _startingInventorySlots = startingInventorySlots;
        _startingActiveItemSlots = startingActiveItemSlots;

        InitializeStatSetters();
    }

    private void InitializeStatSetters()
    {
        _statSetters = new Dictionary<StatName, Action<float>>
        {
            { StatName.Level, value => _level = value },
            { StatName.Health, value => _health = value },
            { StatName.Energy, value => _energy = value },
            { StatName.EnergyRechargeAmmount, value => _energyRechargeAmmount = value },
            { StatName.Armor, value => _armor = value },
            { StatName.MovementSpeed, value => _movementSpeed = value },
            { StatName.MovementRotationSpeed, value => _movementRotationSpeed = value },
            { StatName.SprintSpeed, value => _sprintSpeed = value },
            { StatName.SprintCost, value => _sprintCost = value },
            { StatName.GravityValue, value => _gravityValue = value },
            { StatName.AimAtObjectRange, value => _aimAtObjectRange = value },
            { StatName.JumpHeight, value => _jumpHeight = value },
            { StatName.JumpDelay, value => _jumpDelay = value },
            { StatName.JumpResetDelay, value => _jumpResetDelay = value },
            { StatName.JumpCost, value => _jumpCost = value },
            { StatName.Jumps, value => _maxJumps = (int)value },
            { StatName.AttackDamageModifier, value => _attackDamageModifier = value },
            { StatName.AttackVelocityModifier, value => _attackVelocityModifier = value },
            { StatName.CriticalChanceModifier, value => _criticalChanceModifier = value },
            { StatName.CriticalDamageModifier, value => _criticalDamageModifier = value },
            { StatName.ArmorPiercingModifier, value => _armorPiercingModifier = value },
            { StatName.AmmoPoolModifier, value => _ammoPoolModifier = (int)value },
            { StatName.InventorySlots, value => _inventorySlots = (int)value },
            { StatName.ActiveItemSlots, value => _activeItemSlots = (int)value },
            { StatName.StartingInventorySlots, value => _startingInventorySlots = (int)value },
            { StatName.StartingActiveItemSlots, value => _startingActiveItemSlots = (int)value }
        };
    }

    public void SetPlayerStat(StatName stat, float value)
    {
        if (_statSetters.TryGetValue(stat, out var setter))
        {
            setter(value);
        }
        else
        {
            DebugLogger.Log($"Stat name {stat} not found", DebugLogger.LogStyle.Normal, Color.red);
        }
    }
}