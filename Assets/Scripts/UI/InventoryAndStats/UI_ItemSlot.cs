using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI Slot Setup")]
    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI itemStackSize;
    protected UI ui;
    protected RectTransform rect;

    public Inventory_Item itemInSlot { get; private set; }
    protected Inventory_Player inventory;


    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        inventory = FindAnyObjectByType<Inventory_Player>();    
        rect = GetComponent<RectTransform>();
    }

    public void UpdateSlot(Inventory_Item item)
    {
        itemInSlot = item;

        if (itemInSlot == null)
        {
            itemStackSize.text = "";
            itemIcon.color = Color.clear;
            return;
        }

        Color color = Color.white;
        color.a = .9f;
        itemIcon.color = color;
        itemIcon.sprite = itemInSlot.itemData.icon;
        itemStackSize.text = itemInSlot.stackSize > 1 ? itemInSlot.stackSize.ToString() : "";
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (itemInSlot == null || itemInSlot.itemData.type == ItemType.Material)
            return;

        if (itemInSlot.itemData.type == ItemType.Consumable)
        {
            if (itemInSlot.itemEffect.CanBeUsed())
                inventory.TryUseItem(itemInSlot);
        }
        else
            inventory.TryEquipItem(itemInSlot);
        
        ui.itemToolTip.ShowTooltip(false, null);
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (itemInSlot == null) return;

        ui.itemToolTip.ShowTooltip(true, rect, itemInSlot);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (itemInSlot == null) return;

        ui.itemToolTip.ShowTooltip(false, null);
    }
}
