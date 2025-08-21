using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Details", menuName = "Red Horizon/New Weapon Detail", order = 1)]
public class WeaponDetails : ScriptableObject
{
    [Tooltip("Give this weapon a unique name")]
    public  new string name;
    [Tooltip("Whether or not this weapon is handheld or ranged")]
    public bool isMelee;
    [Tooltip("Should this weapon use Hitscan or Projectile?")]
    public bool usesHitscan;
    [Tooltip("How much damage per projectile this weapon deals")]
    public float damage;
    [Tooltip("How far in Unity units the projectile can travel")]
    public float range;
    [Tooltip("How fast the projectile (if any) travels")]
    public float velocity;
    [Tooltip("The delay between shooting (if weapon is ranged) in seconds")]
    public float speed;
    [Tooltip("Total ammount of ammunition a player can hold in reserves")]
    public int ammoPool;
    [Tooltip("Total ammount of ammunition a player can hold in their magizne")]
    public int  magazineSize;
    [Space(5)]
    [Tooltip("The Prefab of the weapon you want to instantiate for this weapon")]
    public GameObject weaponModel;
    [Tooltip("The Prefab of the projectile (if any) you want to instantiate for this weapon")]
    public GameObject weaponProjectile;
    [Tooltip("The decal that will be instantiated upon shot fired contact (if ranged)")]
    public Sprite impactDecal;
}

