using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firearm : Weapon
{
    #region Members
    [SerializeField] private Transform _shotPoint;
    [SerializeField] private ParticleSystem _gunShotFX;
    [SerializeField] private WeaponDetails _details;
    [SerializeField] private LayerMask _playerLayerMask;
    [SerializeField] private LayerMask _enemyLayerMast;
    private const int EnemyLayer = 8;
    private const int EnemyWeakSpotLayer = 9;
    private WaitForSeconds _shotDuration = new WaitForSeconds(0.07f);
    private LineRenderer _lineRenderer;
    private GameObject _aimAtObject;
    private Coroutine attackCoroutine;
    private WaitForSeconds _rateOfFireWait;
    private Camera _cam;
    #endregion

    #region Monobehaviours
    private void Awake()
    {
        WeaponDetail = _details;
    }
    private void OnEnable()
    {
        Subscribe();
    }
    private void Start()
    {
        SetReferences();
        SetStartingAttributes();
    }
    private void OnDisable()
    {
        Unsubscribe();
    }
    #endregion

    #region Private
    private void Subscribe()
    {
        AttackStart += StartAttack;
        AttackStop += StopAttack;
    }
    private void Unsubscribe()
    {
        AttackStart -= StartAttack;
        AttackStop -= StopAttack;
    }
    private void SetReferences()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _cam = Camera.main;
        StartCoroutine(SetReferencesRoutine());
    }
    private IEnumerator SetReferencesRoutine()
    {
        while (_aimAtObject == null)
        {
            try
            {
                _aimAtObject = GameObject.FindGameObjectWithTag("Aim At Object");
            }
            catch (Exception e)
            {
                Debug.LogError("Couldn't set references in Firearm.cs: " + e.Message);
            }
            yield return null;
        }
    }
    private void StartAttack()
    {
        attackCoroutine = StartCoroutine(AutoFire());
    }
    private void StopAttack()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
    }
    public void SetAimTarget(GameObject aim)
    {
        _aimAtObject = aim;
    }
    private void SetStartingAttributes()
    {
        ammoInMag = WeaponDetail.magazineSize;
        curAmmo = WeaponDetail.ammoPool;
    }
    private void FireProjectile()
    {
        Vector3 rayOrigin = _cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        RaycastHit hit;

        //_lineRenderer.SetPosition(0, _shotPoint.transform.position);

        if (Physics.Raycast(rayOrigin, _cam.transform.forward, out hit, _details.range))
        {
            //_lineRenderer.SetPosition(1, hit.point);
            HandleImpact(hit);
        }
        else
        {
            //_lineRenderer.SetPosition(1, rayOrigin + (_cam.transform.forward * _details.range));
        }
        _gunShotFX.Play();
        GameManager.Instance.ServiceLocator.EventManager.OnWeaponFired.Invoke();
        //StartCoroutine(LineRenderingRoutine());
    }

    private IEnumerator AutoFire()
    {
        _rateOfFireWait = new WaitForSeconds(1 / WeaponDetail.speed);

        while (ammoInMag > 0)
        {
            FireProjectile();
            yield return _rateOfFireWait;
        }
        yield return null;
    }
    private void ReloadWeapon()
    {
        int requiredAmmo = WeaponDetail.magazineSize - ammoInMag;
        int difference = curAmmo - requiredAmmo;
        if (difference >= 0)
        {
            curAmmo -= requiredAmmo;
            ammoInMag += requiredAmmo;
        }
        else if (difference < 0)
        {
            ammoInMag += curAmmo;
            curAmmo = 0;
        }
        GameManager.Instance.ServiceLocator.EventManager.OnWeaponReloaded.Invoke();
    }

    private void HandleImpact(RaycastHit hit)
    {
        int colliderLayer = hit.collider.transform.gameObject.layer;
        
        ammoInMag--;
        if (ammoInMag < 0)
        {
            ammoInMag = 0;
        }
        // if we hit an enemy's body or arms
        if (colliderLayer == EnemyLayer)
        {
           DealDamage(_details.damage, hit.transform.gameObject, hit.point);
        }
        // if we hit an enemy weak spot
        else if (colliderLayer == EnemyWeakSpotLayer)
        {
            DealDamage(_details.damage, hit.transform.gameObject, hit.point, true);
        }
    }
    private IEnumerator LineRenderingRoutine()
    {
        _lineRenderer.enabled = true;
        yield return _shotDuration;
        _lineRenderer.enabled = false;
    }
    private void Knockback(RaycastHit hit)
    {
        if (hit.rigidbody != null)
        {
            hit.rigidbody.AddForce(-hit.normal * 100);
        }
    }
    #endregion

    #region Public
    public override void AddAmmoToPool(int amnt)
    {
        if (curAmmo < WeaponDetail.ammoPool)
        {
            curAmmo += amnt;
            if (curAmmo > WeaponDetail.ammoPool)
            {
                curAmmo = WeaponDetail.ammoPool;
            }
        }
    }
    public override void RemoveAmmoFromPool(int amnt)
    {
        if (curAmmo > WeaponDetail.ammoPool)
        {
            curAmmo -= amnt;
            if (curAmmo < 0)
            {
                curAmmo = 0;
            }
        }
    }
    public override void ReloadMag()
    {
        ReloadWeapon();
    }
    #endregion 
}

