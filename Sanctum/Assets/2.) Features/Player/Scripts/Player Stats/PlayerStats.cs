using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [Serializable]
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

        #region Members

        // Gameplay
         private float _level;
        // Vitals
        private float _health;
        private float _energy;
        private float _energyRechargeAmmount;
        private float _armor;
        // Movement modifiers
        private float _movementSpeed;
        private float _movementRotationSpeed;
        private float _sprintSpeed;
        private float _sprintCost;
        private float _gravityValue;
        private float _aimAtObjectRange;
        private float _jumpHeight;
        private float _jumpDelay;
        private float _jumpResetDelay;
        private float _jumpCost;
        private int _maxJumps;
        // Damage modifiers
        private float _attackDamageModifier;
        private float _attackVelocityModifier;
        private float _criticalChanceModifier;
        private float _criticalDamageModifier;
        private float _armorPiercingModifier;
        private int _ammoPoolModifier;
        // Items
        private int _inventorySlots;
        private int _activeItemSlots;
        private int _startingInventorySlots;
        private int _startingActiveItemSlots;

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
            float movementRotationSpeed, float sprintSpeed, float sprintCost, float gravityValue,
            float aimAtObjectRange,
            float jumpHeight, float jumpDelay, float jumpResetDelay, float jumpCost, int maxJumps,
            float attackDamageModifier,
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
                DebugLogger.Log($"Player Stat name {stat} not found", DebugLogger.LogStyle.Normal, Color.red);
            }
        }
    }
}