using TMPro;
using UnityEngine;

public class Item : MonoBehaviour
{
    #region Members
    [SerializeField] private ItemTemplate _itemDetails;
    public ItemTemplate ItemDetails { get { return _itemDetails; } }
    private PlayerControls _playerInputActions;
    private PlayerInventoryManager _playerInventoryManager;
    private bool _inTrigger = false;
    #endregion

    #region Monobehaviours
    private void Awake()
    {
        Initialize();
    }
    private void OnEnable()
    {
        _playerInputActions.Player.Interact.performed += _ => TryGetItem();
    }
    private void OnDisable()
    {
        _playerInputActions.Player.Interact.performed -= _ => TryGetItem();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _inTrigger = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _inTrigger = false;
        }
    }
    #endregion

    #region Private
    private void Initialize()
    {
        _playerInputActions = new PlayerControls();
        _playerInputActions.Player.Enable();
        if (_playerInventoryManager == null)
        {
            _playerInventoryManager = GameManager.Instance.ServiceLocator.GetService<PlayerInventoryManager>();
        }
    }
    
    private void TryGetItem()
    {
        if (_inTrigger && _playerInventoryManager.AddItemToInventory(_itemDetails))
        {
            Destroy(gameObject);
        }
    }
    #endregion

}

