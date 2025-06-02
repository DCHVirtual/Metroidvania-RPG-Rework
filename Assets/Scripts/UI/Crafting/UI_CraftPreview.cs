using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftPreview : MonoBehaviour
{
    Inventory_Item itemToCraft;
    Inventory_Storage storage;
    UI_CraftMaterialSlot[] materialSlots;
    UI ui;

    [Header("Item Preview Setup")]
    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemInfo;
    [SerializeField] TextMeshProUGUI craftButton;

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
    }

    public void AttemptCraft()
    {
        if (itemToCraft == null)
        {
            craftButton.text = "Pick an item";
            return;
        }

        if (storage.HasEnoughToCraft(itemToCraft) && storage.playerInventory.CanAddItemToInventory(itemToCraft))
        {
            storage.CraftItem(itemToCraft);
        }

        UpdateCraftPreviewSlots();
    }

    public void SetupCraftPreview(Inventory_Storage storage)
    {
        this.storage = storage;

        materialSlots = GetComponentsInChildren<UI_CraftMaterialSlot>();
        foreach (var slot in materialSlots)
            slot.gameObject.SetActive(false);
    }

    public void UpdateCraftPreview(Data_ItemSO itemData)
    {
        itemToCraft = new Inventory_Item(itemData);

        itemIcon.sprite = itemData.icon;
        itemName.text = itemData.itemName;
        itemInfo.text = ui.GetItemInfo(itemToCraft);
        UpdateCraftPreviewSlots();
    }

    private void UpdateCraftPreviewSlots()
    {
        foreach (var slot in materialSlots)
            slot.gameObject.SetActive(false);

        for (int i = 0; i < itemToCraft.itemData.craftRecipe.Length; i++)
        {
            var requiredItem = itemToCraft.itemData.craftRecipe[i];
            int availableAmount = storage.GetAvailableAmountOf(requiredItem.itemData);
            int requiredAmount = itemToCraft.itemData.craftRecipe[i].stackSize;
            materialSlots[i].gameObject.SetActive(true);
            materialSlots[i].SetupMaterialSlot(requiredItem.itemData, availableAmount, requiredAmount);
        }
    }
}
