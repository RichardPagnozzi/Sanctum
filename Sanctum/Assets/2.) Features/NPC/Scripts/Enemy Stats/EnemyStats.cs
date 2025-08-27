using System;
using System.Collections.Generic;
using UnityEngine;

namespace NPC.Enemy
{
    public class EnemyStats
    {
        [NonSerialized] private Dictionary<StatName, Action<float>> _statSetters;

        public enum StatName
        {
            Health,
            Armor,
            WalkingSpeed,
            ChasingSpeed,
            RotationSpeed,
            AttackDamageBase,
            AttackDamageModifier,
            WeakspotMultiplier,
        }

        // Private References that get changed under the hood
        private float _health;
        private float _armor;
        private float _walkingSpeed;
        private float _chasingSpeed;
        private float _rotationSpeed;
        private float _attackDamageBase;
        private float _attackDamageModifier;
        private float _weakspotMultiplier;

        // Public Properties that get read by systems
        public float Health
        {
            get => _health;
        }

        public float Armor
        {
            get => _armor;
        }

        public float WalkingSpeed
        {
            get => _walkingSpeed;
        }

        public float ChasingSpeed
        {
            get => _chasingSpeed;
        }

        public float RotationSpeed
        {
            get => _rotationSpeed;
        }

        public float AttackDamageBase
        {
            get => _attackDamageBase;
        }

        public float AttackDamageModifier
        {
            get => _attackDamageModifier;
        }

        public float WeakspotMultiplier
        {
            get => _weakspotMultiplier;
        }

        public EnemyStats(
            float health, float armor, float walkingSpeed, float chasingSpeed, float rotationSpeed,
            float attackDamageBase, float attackDamageModifier, float weakspotMultiplier)
        {
            _health = health;
            _armor = armor;
            _walkingSpeed = walkingSpeed;
            _chasingSpeed = chasingSpeed;
            _rotationSpeed = rotationSpeed;
            _attackDamageBase = attackDamageBase;
            _attackDamageModifier = attackDamageModifier;
            _weakspotMultiplier = weakspotMultiplier;
            
            InitializeStatSetters();
        }

        private void InitializeStatSetters()
        {
            _statSetters = new Dictionary<StatName, Action<float>>
            {
                { StatName.Health, value => _health = value },
                { StatName.Armor, value => _armor = value },
                { StatName.WalkingSpeed, value => _walkingSpeed = value },
                { StatName.ChasingSpeed, value => _chasingSpeed = value },
                { StatName.RotationSpeed, value => _rotationSpeed = value },
                { StatName.AttackDamageModifier, value => _attackDamageModifier = value },
                { StatName.AttackDamageBase, value => _attackDamageBase = value },
                { StatName.WeakspotMultiplier , value => _weakspotMultiplier = value}
            };
        }

        public void SetEnemyStat(StatName stat, float value)
        {
            if (_statSetters.TryGetValue(stat, out var setter))
            {
                setter(value);
            }
            else
            {
                DebugLogger.Log($"Enemy Stat name {stat} not found", DebugLogger.LogStyle.Normal, Color.red);
            }
        }
    }
}