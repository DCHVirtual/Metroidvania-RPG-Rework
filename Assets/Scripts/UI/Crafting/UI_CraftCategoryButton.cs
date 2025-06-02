using UnityEngine;

public class UI_CraftCategoryButton : MonoBehaviour
{
    [SerializeField] Data_ItemListSO craftData;
    UI_CraftBookSlot[] craftSlots;

    public void SetCraftSlots(UI_CraftBookSlot[] craftSlots) => this.craftSlots = craftSlots;
    
    public void UpdateCraftSlots()
    {
        foreach (var craftSlot in craftSlots)
        {
            craftSlot.gameObject.SetActive(false);
        }

        for (int i = 0; i < craftData.itemList.Length; i++)
        {
            Data_ItemSO itemData = craftData.itemList[i];

            craftSlots[i].gameObject.SetActive(true);
            craftSlots[i].SetupButton(itemData);
        }
    }
}
