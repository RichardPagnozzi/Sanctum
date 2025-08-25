using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerInventoryManager : PlayerBase
    {
        #region Members

        public struct EquippedWeaponReference
        {
            public void SetDetails(GameObject obj, ItemTemplate item)
            {
                ItemModel = obj;
                ItemTemplate = item;
            }

            public GameObject ItemModel;
            public ItemTemplate ItemTemplate;
        }

        [SerializeField] private Transform _armorSpawnPoint;
        [SerializeField] private Transform _weaponParent;
        [SerializeField] private ItemTemplate _startingWeaponItem;
        public List<ItemTemplate> Items { get; private set; }

        private const int MaxInventorySlots = 20;
        private const int BaseInventorySlots = 5;
        private int _curMaxInventorySlots;
        private int _curAvailableItemSlots;
        private EquippedWeaponReference _equippedWeaponRef = new EquippedWeaponReference();
        private bool _displayInventory;

        #endregion

        #region MonoBehaviour Methods

        private void Awake()
        {
            if (TryGetPlayerDetails())
            {
                _curMaxInventorySlots = BaseInventorySlots;
                _displayInventory = false;
            }
            else
            {
                enabled = false;
            }
        }

        private void Start()
        {
            Items = new List<ItemTemplate>();
            _curAvailableItemSlots = _curMaxInventorySlots;

            // If manually given a weapon in the scene view
            if (_weaponParent.transform.childCount == 3)
            {
                int index = _weaponParent.transform.childCount - 1;
                GameObject obj = _weaponParent.transform.GetChild(index).gameObject;
                Weapon weapon = obj.GetComponent<Weapon>();

                _equippedWeaponRef.SetDetails(obj, weapon.GetAssociatedItemTemplate());
                GetComponent<PlayerAttackManager>().SetEquippedWeapon(weapon);
                AddItemToInventory(_equippedWeaponRef.ItemTemplate);
            }
            // If no item is manually added, programatically equip one from here
            else
            {
                AddItemToInventory(_startingWeaponItem);
            }
        }

        public void OnInventoryToggled()
        {
            _displayInventory = !_displayInventory;
        }

        private void OnGUI()
        {
            if (_displayInventory)
            {
                const int boxWidth = 200;
                const int boxHeight = 25;
                GUI.Box(new Rect(10, 10, boxWidth, 30), "Player Inventory");
                for (int i = 0; i < Items.Count; i++)
                {
                    GUI.Box(new Rect(10, 40 + (i * boxHeight), boxWidth, boxHeight),
                        Items[i].Type + ": " + Items[i].Name);
                }
            }
        }

        #endregion

        #region Private Methods

        private void DestroyEquippedWeaponObject()
        {
            Destroy(_weaponParent.GetChild(_weaponParent.childCount - 1).gameObject);
        }

        private void EquipItem(ItemTemplate item)
        {
            switch (item.Type)
            {
                case KeywordDictionary.ItemType.Weapon:
                {
                    if (_equippedWeaponRef.ItemModel != null)
                    {
                        // This destroys the physical instance of the weapon from the scene
                        DestroyEquippedWeaponObject();

                        // This removes the item completely from the inventory
                        RemoveItemFromInventory(_equippedWeaponRef.ItemTemplate);
                    }

                    _equippedWeaponRef.SetDetails(Instantiate(item.Prefab, _weaponParent), item);
                    GetComponent<PlayerAttackManager>()
                        .SetEquippedWeapon(_equippedWeaponRef.ItemModel.GetComponent<Weapon>());

                    break;
                }
                case KeywordDictionary.ItemType.Armor:
                {
                    GameObject reference = Instantiate(item.Prefab, _armorSpawnPoint);
                    break;
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns True if an item was successfully added to the players inventory
        /// </summary>
        /// <param name="item">Specific Item Type</param>
        /// <returns>Returns True if an item was successfully added to the players inventory</returns>
        public bool AddItemToInventory(ItemTemplate item)
        {
            if (Items.Count < _curAvailableItemSlots && !Items.Contains(item))
            {
                // Add Item To Inventory
                Items.Add(item);

                // Equip Item if it's equipable  
                if (item.Type == KeywordDictionary.ItemType.Weapon || item.Type == KeywordDictionary.ItemType.Armor)
                {
                    Debug.Log("Weapon/Armor Added");
                    EquipItem(item);
                }
                else
                {
                    if (item.Type == KeywordDictionary.ItemType.Consumable)
                    {
                        Debug.Log("Consumble Added");
                    }
                    else if (item.Type == KeywordDictionary.ItemType.Perk)
                    {
                        Debug.Log("Perk Added");
                    }
                }

                GameManager.Instance.ServiceLocator.EventManager.OnItemAdded?.Invoke(item);
                return true;
            }

            GameManager.Instance.ServiceLocator.EventManager.OnItemAddedFailed?.Invoke();
            return false;
        }

        /// <summary>
        /// Removes an Item from the inventory if it exists
        /// </summary>
        /// <param name="item">The item to be removed</param>
        public void RemoveItemFromInventory(ItemTemplate item)
        {
            if (Items.Remove(item))
            {
                Debug.Log("Item Removed From Inventory");
                GameManager.Instance.ServiceLocator.EventManager.OnItemRemoved?.Invoke(item);
            }
        }

        #endregion
    }
}