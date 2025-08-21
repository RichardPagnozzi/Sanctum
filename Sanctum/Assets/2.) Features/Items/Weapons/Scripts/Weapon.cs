using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BlazeAISpace;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private ItemTemplate _associatedItemTemplate;
    public WeaponDetails WeaponDetail { get; set; }
    public int ammoInMag { get; set; }
    public int curAmmo { get; set; }

    //Getters
    public ItemTemplate GetAssociatedItemTemplate()
    {
        return _associatedItemTemplate;
    }

    // Events
    public Action AttackStart;
    public Action AttackStop;

    // Dealing Damage
    public virtual void DealDamage(float amnt, GameObject reciever, Vector3 hitPosition, bool isWeakSpot = false)
    {
        GameManager.Instance.ServiceLocator.EventManager.OnEnemyRecieveDamage.Invoke(amnt, reciever, hitPosition, isWeakSpot);
    }

    // Reloading Weapon
    public abstract void AddAmmoToPool(int amnt);
    public abstract void RemoveAmmoFromPool(int amnt);
    public abstract void ReloadMag();
}
