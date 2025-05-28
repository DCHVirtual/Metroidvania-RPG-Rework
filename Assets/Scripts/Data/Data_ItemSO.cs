using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Material Data", fileName = "Material Data - ")]
public class Data_ItemSO : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public ItemType itemType;
    public int maxStackSize = 1;
}
