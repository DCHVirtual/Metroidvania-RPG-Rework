#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Material Item", fileName = "Material Data - ")]
public class Data_ItemSO : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public ItemType type;
    public int maxStackSize = 1;

    [Header("Item Effect")]
    public Data_ItemEffectSO effect;

    [Header("Craft Details")]
    public Inventory_Item[] craftRecipe;

    [Header("Merchant Details")]
    public int price = 100;
    public int minShopStackSize = 1;
    //public int maxShopStackSize = 1;

    [Header("Drop Details")]
    [Range(0, 1000)]
    public int itemRarity = 100;
    [Range(0, 100f)]
    public float dropChance;

    [field: SerializeField] public string saveID { get; private set; }

#if UNITY_EDITOR
    private void OnValidate()
    {
        string path = AssetDatabase.GetAssetPath(this);
        saveID = AssetDatabase.AssetPathToGUID(path);
    }
#endif
}
