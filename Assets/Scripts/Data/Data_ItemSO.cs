using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Material Item", fileName = "Material Data - ")]
public class Data_ItemSO : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public ItemType type;
    public ItemRarity rarity;
    public int maxStackSize = 1;

    [Header("Item Effect")]
    public Data_ItemEffectSO effect;

    [Header("Craft Details")]
    public Inventory_Item[] craftRecipe;
}
