using UnityEngine;

public class Object_ItemPickup : MonoBehaviour
{
    [SerializeField] Vector2 dropForce = new Vector2(3.5f, 10f);
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Collider2D col;

    [SerializeField] Data_ItemSO itemData;

    Inventory_Item itemToAdd;

    private void Awake()
    {
        itemToAdd = new Inventory_Item(itemData);
    }

    private void OnValidate()
    {
        if (itemData == null) return;

        SetupVisuals();
        //sr = GetComponent<SpriteRenderer>();
        
    }

    public void SetupItem(Data_ItemSO itemData)
    {
        this.itemData = itemData;
        SetupVisuals();

        float xForce = Random.Range(-dropForce.x, dropForce.x);
        rb.linearVelocity = new Vector2(xForce, dropForce.y);
        col.isTrigger = false;
    }

    void SetupVisuals()
    {
        sr.sprite = itemData.icon;
        gameObject.name = $"Object_ItemPickup - {itemData.itemName}";
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && !col.isTrigger)
        {
            col.isTrigger = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var inventory = collision.GetComponent<Inventory_Player>();

        if (inventory != null)
        {
            var itemToAdd = new Inventory_Item(itemData);
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
}
