using UnityEngine;

public class Object_ItemPickup : MonoBehaviour
{
    SpriteRenderer sr;
    [SerializeField] Data_ItemSO itemData;

    Inventory inventory;
    Inventory_Item itemToAdd;

    private void Awake()
    {
        itemToAdd = new Inventory_Item(itemData);
    }

    private void OnValidate()
    {
        if (itemData == null) return;

        sr = GetComponent<SpriteRenderer>();
        sr.sprite = itemData.itemIcon;
        gameObject.name = $"Object_ItemPickup - {itemData.itemName}";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        inventory = collision.GetComponent<Inventory>();

        if (inventory != null && inventory.CanAddItem(itemToAdd))
        {
            inventory.AddItem(itemToAdd);
            Destroy(gameObject);
        }
    }
}
