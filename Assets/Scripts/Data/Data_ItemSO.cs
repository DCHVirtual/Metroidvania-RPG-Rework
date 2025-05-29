using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Material Data", fileName = "Material Data - ")]
public class Data_ItemSO : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public ItemType type;
    public ItemRarity rarity;
    public int maxStackSize = 1;
}
