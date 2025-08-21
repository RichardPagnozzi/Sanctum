using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingPlatformController : MonoBehaviour
{
    public enum ChargeType { HEALTH, ARMOR };
    [SerializeField] private ChargeType _chargeType;
    [SerializeField] private GameObject _emmisiveObject;
    [SerializeField] private Material _matHealth;
    [SerializeField] private Material _matArmor;
    private float _chargeAmnt;
    private bool _isCharging;

    private void Start()
    {
        _isCharging = false;
        _chargeAmnt = 1;
    }
    private void OnEnable()
    {
        switch(_chargeType)
        {
            case ChargeType.HEALTH:
                {
                    _emmisiveObject.GetComponent<MeshRenderer>().material = _matHealth;
                    break;
                }
            case ChargeType.ARMOR:
                {
                    _emmisiveObject.GetComponent<MeshRenderer>().material = _matArmor;
                    break;
                }
            default:
                break;
        }
    }
    private IEnumerator RechargeRoutine()
    {
        while (_isCharging)
        {
            yield return new WaitForSeconds(0.1f);
            if (_chargeType == ChargeType.HEALTH)
            {
                GameManager.Instance.ServiceLocator.EventManager.OnPlayerRecieveHealth.Invoke(_chargeAmnt);
            }
            else if(_chargeType == ChargeType.ARMOR)
            {
                GameManager.Instance.ServiceLocator.EventManager.OnPlayerRecieveArmor.Invoke(_chargeAmnt);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _isCharging = true;
            StartCoroutine(RechargeRoutine());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _isCharging = false;
        }
    }
}
