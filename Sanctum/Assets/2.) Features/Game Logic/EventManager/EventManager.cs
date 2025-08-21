using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager
{
    // Rooms
    public Action OnRoomEnter;
    public Action OnRoomExit;
    // Player Universal
    public Action OnPlayerHealthModified;
    public Action OnPlayerArmorModified;
    public Action OnPlayerEnergyModified;
    public Action OnPlayerIsDead;
    // Player Health
    public Action<float, int> OnPlayerRecieveTickDamage;
    public Action<float> OnPlayerRecieveDamage;
    public Action<float, int> OnPlayerRecieveTickHealth;
    public Action<float> OnPlayerRecieveHealth;
    public Action<float> OnPlayerIncreaseMaxHealth;
    // Player Armor
    public Action<float> OnPlayerRecieveArmor;
    public Action<float> OnPlayerIncreaseMaxArmor;
    // Player Energy
    public Action<float> OnPlayerRecieveEnergy;
    public Action<float> OnPlayerLoseEnergy;
    public Action<float, int> OnPlayerRecieveTickEnergy;
    public Action<float> OnPlayerIncreaseMaxEnergy;
    // Player Attack
    public Action OnPlayerAttackStart;
    public Action OnPlayerAttackStop;
    // Enemy Health
    public Action<float, GameObject, Vector3, bool> OnEnemyRecieveDamage;
    public Action<float> OnEnemyRecieveHealth;
    // Items
    public Action<ItemTemplate> OnItemAdded;
    public Action OnItemAddedFailed;
    public Action <ItemTemplate>OnItemRemoved;
    public Action<Weapon> OnWeaponEquipped;
    public Action OnWeaponFired;
    public Action OnWeaponReloaded;
}
