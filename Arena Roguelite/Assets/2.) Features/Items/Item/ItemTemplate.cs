using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Item", menuName = "Red Horizon/New Item Template", order = 1)]
public class ItemTemplate : ScriptableObject
{
    public KeywordDictionary.ItemType Type; 
    public string Name;
    public int Value;
    public int EquippedEnergyCost;
    public GameObject Prefab;
    public Sprite InventoryIcon;
    public Sprite ActiveInventoryIcon;
}