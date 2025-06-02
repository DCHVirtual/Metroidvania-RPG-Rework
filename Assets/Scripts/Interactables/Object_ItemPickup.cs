using UnityEngine;

public class Object_ItemPickup : MonoBehaviour
{
    SpriteRenderer sr;
    [SerializeField] Data_ItemSO itemData;

    Inventory_Item itemToAdd;

    private void Awake()
    {
        itemToAdd = new Inventory_Item(itemData);
    }

    private void OnValidate()
    {
        if (itemData == null) return;

        sr = GetComponent<SpriteRenderer>();
        sr.sprite = itemData.icon;
        gameObject.name = $"Object_ItemPickup - {itemData.itemName}";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var itemToAdd = new Inventory_Item(itemData);
        var inventory = collision.GetComponent<Inventory_Player>();
        var storage = inventory.storage;

        if (itemData.type == ItemType.Material)
        {
            storage.AddMaterialToStash(itemToAdd);
            Destroy(gameObject);
        }
        else if (inventory.CanAddItemToInventory(itemToAdd))
        {
            inventory.AddItemToInventory(itemToAdd);
            Destroy(gameObject);
        }
    }
}
