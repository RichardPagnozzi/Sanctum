using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using DamageNumbersPro;

namespace NPC.Enemy
{
    public class EnemyStatusManager : MonoBehaviour, IPoolReturn
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

        [SerializeField] private KeywordDictionary.EnemyType _type;
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private Slider _armorSlider;
        [SerializeField] private DamageNumber _damagePopup;
        [SerializeField] private ParticleSystem _bloodSplatter;
        private EnemySpawnController _spawner;
        private EnemyDetails _enemyDetails;
        private float _dmgAmnt;
        private const float DeathDelay = 10;

        public float CurEnemyHealth { get; private set; }
        public float MaxEnemyHealth { get; private set; }
        public float CurEnemyArmor { get; private set; }
        public float MaxEnemyArmor { get; private set; }

        private RectTransform _damagePopupCanvas;
        private bool _showStatus;

        #endregion

        #region Monobehaviours

        private void Awake()
        {
            AssignBaseStats();

            _damagePopupCanvas = GameManager.Instance.ServiceLocator.GetService<ScreenCollector>()
                .GetDamageNumberContainer().GetComponent<RectTransform>();
        }

        private void Start()
        {
            GameManager.Instance.ServiceLocator.EventManager.OnEnemyRecieveDamage += ReceiveDamage;
            GameManager.Instance.ServiceLocator.EventManager.OnEnemyRecieveHealth += ReceiveHealth;
        }

        private void OnEnable()
        {
            ShowStatus = _showStatus;
        }

        private void OnDestroy()
        {
            GameManager.Instance.ServiceLocator.EventManager.OnEnemyRecieveDamage -= ReceiveDamage;
            GameManager.Instance.ServiceLocator.EventManager.OnEnemyRecieveHealth -= ReceiveHealth;
        }

        #endregion

        #region Private

        // Threshold Checks
        private void EnemyDeathCheck()
        {
            if (CurEnemyHealth <= 0)
            {
                GetComponent<BlazeAI>().Death(true, this.gameObject);
                StartCoroutine(DeathCoroutine());
            }
        }

        private void EnemyNegativeHealthCheck()
        {
            if (CurEnemyHealth <= 0)
            {
                CurEnemyHealth = 0;
            }
        }

        private void EnemyMaxHealthCheck()
        {
            if (CurEnemyHealth >= MaxEnemyHealth)
            {
                CurEnemyHealth = MaxEnemyHealth;
            }
        }

        private void ReceiveDamage(float dmg, GameObject reciever, Vector3 hitPosition, bool isWeakSpot)
        {
            if (reciever == gameObject)
            {
                float rawDamage = dmg;
                float trueDamage;
                Color damageNumberColor;

                // Decide Damage Amnt
                if (isWeakSpot)
                {
                    trueDamage = rawDamage * _enemyDetails.Stats.WeakspotMultiplier;
                    damageNumberColor = Color.yellow;
                }
                else
                {
                    trueDamage = dmg;
                    damageNumberColor = Color.white;
                }

                // Deal Damage
                CurEnemyHealth -= trueDamage;

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

        private void ReceiveHealth(float health)
        {
            if (CurEnemyHealth == MaxEnemyHealth)
            {
                return;
            }

            CurEnemyHealth += health;
            EnemyMaxHealthCheck();
        }

        private void IncreaseMaxHealth(float health)
        {
            MaxEnemyHealth += health;
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
            if (_spawner != null && _spawner.usePooling)
            {
                _spawner.ReturnToPool(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void UpdateSliders()
        {
            _healthSlider.value = CurEnemyHealth / MaxEnemyHealth;
            _armorSlider.value = CurEnemyArmor / MaxEnemyArmor;
        }

        private void AssignBaseStats()
        {
            _enemyDetails = new EnemyDetails(_type, KeywordDictionary.EnemyArchType.Standard);
            CurEnemyHealth = MaxEnemyHealth = _enemyDetails.Stats.Health;
            CurEnemyArmor = MaxEnemyArmor = _enemyDetails.Stats.Armor;
            _dmgAmnt = _enemyDetails.Stats.AttackDamageBase * _enemyDetails.Stats.AttackDamageModifier;
        }

        #endregion

        #region Public

        public void AttackPlayer()
        {
            try
            {
                GameManager.Instance.ServiceLocator.EventManager.OnPlayerRecieveDamage.Invoke(_dmgAmnt);
            }
            catch (Exception)
            {
            }

            Debug.Log("Attacking Player");
        }

        public void SetupReturn(EnemySpawnController spawner)
        {
            _spawner = spawner;
        }

        #endregion
    }
}