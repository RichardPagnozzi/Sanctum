using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class LootChestController : MonoBehaviour
{
    [SerializeField] LootChest _lootChest;
    [SerializeField] private Transform _itemSpawnPoint;
    [SerializeField] private WeightedRandomList<GameObject> _lootTable;
    private PlayerControls _playerInputActions;


    private GameObject spawnedItem;
    private GameObject randomItem;
    private bool _opened = false;
    private bool _inTrigger = false;

    #region Monobehaviours
    private void Awake()
    {
        Initialize();
    }
    private void OnEnable()
    {
        _playerInputActions.Player.Interact.performed +=  TryOpen;
    }
    private void OnDisable()
    {
        _playerInputActions.Player.Interact.performed -=  TryOpen;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _inTrigger = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _inTrigger = false;
        }
    }

    private void Initialize()
    {
        _playerInputActions = new PlayerControls();
        _playerInputActions.Player.Enable();
    }
    #endregion

    private GameObject GetRandomItem()
    {
        return randomItem = _lootTable.GetRandom();
    }
    private void SpawnRandomItem(GameObject item)
    {
        spawnedItem = Instantiate(item, _itemSpawnPoint);
    }
    private void TryOpen(InputAction.CallbackContext context)
    {
        if (!_opened && _inTrigger)
        {
            SpawnRandomItem(GetRandomItem());
            _lootChest.OpenChest();
            _opened = true;
        }
    }
}