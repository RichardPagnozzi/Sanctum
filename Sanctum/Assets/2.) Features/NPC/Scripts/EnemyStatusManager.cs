using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using DamageNumbersPro;

public class EnemyStatusManager : MonoBehaviour
{
    #region Members
    public bool ShowStatus
    {
        get => _showStatus;

        set
        {
            _showStatus = value;
            if (_showStatus)
            {
                _armorSlider.gameObject.SetActive(true);
                _healthSlider.gameObject.SetActive(true);
                UpdateSliders();
            }
            else

            {
                _armorSlider.gameObject.SetActive(false);
                _healthSlider.gameObject.SetActive(false);
            }
        }
    }
    
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private Slider _armorSlider;
    [SerializeField] private DamageNumber _damagePopup;
    [SerializeField] private ParticleSystem _bloodSplatter;
    private const float DeathDelay = 10;
    private const float WeakSpotMultiplier = 1.25f;
    private float _dmgAmnt;
   
    public float _curEnemyHealth { get; private set; }
    public float _maxEnemyHealth { get; private set; }
    public float _curEnemyArmor { get; private set; }
    public float _maxEnemyArmor { get; private set; }

    private RectTransform _damagePopupCanvas;
    private bool _showStatus;

    #endregion
    #region Monobehaviours

    private void Awake()
    {
        _maxEnemyHealth = 50f;
        _curEnemyHealth = _maxEnemyHealth;
        _maxEnemyArmor = 20f;
        _curEnemyArmor = _maxEnemyArmor;
        if (_dmgAmnt == null || _dmgAmnt == 0)
        {
            _dmgAmnt = 10;
        }
        _damagePopupCanvas = GameManager.Instance.ServiceLocator.GetService<ScreenCollector>().GetDamageNumberContainer()
            .GetComponent<RectTransform>();
    }

    private void Start()
    {
        GameManager.Instance.ServiceLocator.EventManager.OnEnemyRecieveDamage += RecieveDamage;
        GameManager.Instance.ServiceLocator.EventManager.OnEnemyRecieveHealth += RecieveHealth;
    }

    private void OnEnable()
    {
        ShowStatus = _showStatus;
    }

    private void OnDestroy()
    {
        GameManager.Instance.ServiceLocator.EventManager.OnEnemyRecieveDamage -= RecieveDamage;
        GameManager.Instance.ServiceLocator.EventManager.OnEnemyRecieveHealth -= RecieveHealth;
    }

    #endregion

    #region Private

    // Threshold Checks
    private void EnemyDeathCheck()
    {
        if (_curEnemyHealth <= 0)
        {
            GetComponent<BlazeAI>().Death(true, this.gameObject);
            StartCoroutine(DeathCoroutine());
        }
    }

    private void EnemyNegativeHealthCheck()
    {
        if (_curEnemyHealth <= 0)
        {
            _curEnemyHealth = 0;
        }
    }

    private void EnemyMaxHealthCheck()
    {
        if (_curEnemyHealth >= _maxEnemyHealth)
        {
            _curEnemyHealth = _maxEnemyHealth;
        }
    }

    // Damage
    private void RecieveDamage(float dmg, GameObject reciever, Vector3 hitPosition, bool isWeakSpot)
    {
        if (reciever == gameObject)
        {
            float rawDamage = dmg;
            float trueDamage;
            Color damageNumberColor;

            // Decide Damage Amnt
            if(isWeakSpot)
            {
                trueDamage = rawDamage * WeakSpotMultiplier;
                damageNumberColor = Color.yellow;
            }
            else
            {
                trueDamage = dmg;
                damageNumberColor = Color.white;
            }
            
            // Deal Damage
            _curEnemyHealth -= trueDamage;
            
            // Damage Number Popup
            DamageNumber damageNumber = _damagePopup.Spawn(Vector3.zero, trueDamage);
            damageNumber.SetColor(damageNumberColor);
            damageNumber.SetToMousePosition(_damagePopupCanvas, null);
            
            //  Blood Splatter
            _bloodSplatter.transform.position = hitPosition;
            _bloodSplatter.Play();
            
            // Trigger the HIT behaviour on our AI
            // TODO I want this to be chance based, depending on the weapons stopping power
            GetComponent<BlazeAI>().Hit(this.gameObject, false);
            
            // Negative Health Checks
            EnemyNegativeHealthCheck();
            EnemyDeathCheck();
        }
    }
    
    private IEnumerator ShowStatusIndicators()
    {
        ShowStatus = true;
        yield return new WaitForSeconds(0.75f);
        ShowStatus = false;
    }
    
    // Health
    private void RecieveHealth(float health)
    {
        if (_curEnemyHealth == _maxEnemyHealth)
        {
            return;
        }

        _curEnemyHealth += health;
        EnemyMaxHealthCheck();
    }
    
    private void IncreaseMaxHealth(float health)
    {
        _maxEnemyHealth += health;
    }

    private IEnumerator DeathCoroutine()
    {
        _showStatus = false;
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
        yield return new WaitForSeconds(DeathDelay);
        Destroy(gameObject);
    }
    
    // Effects
    private void UpdateSliders()
    {
        _healthSlider.value = _curEnemyHealth / _maxEnemyHealth;
        _armorSlider.value = _curEnemyArmor / _maxEnemyArmor;
    }

    public void AttackPlayer()
    {
        GameManager.Instance.ServiceLocator.EventManager.OnPlayerRecieveDamage.Invoke(_dmgAmnt);
        Debug.Log("Attacking Player");
    }
    #endregion
}