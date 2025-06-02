using UnityEngine;

public class NPC_Blacksmith : NPC, IInteractable
{
    Inventory_Player playerInventory;
    Inventory_Storage storage;
    protected override void Awake()
    {
        base.Awake();
        storage = GetComponent<Inventory_Storage>();
        anim.SetBool("Blacksmith", true);
    }

    public void Interact()
    {
        storage.SetInventory(playerInventory);
        ui.uiStorage.SetupStorage(storage);
        ui.uiStorage.gameObject.SetActive(true);
        ui.uiCraft.gameObject.SetActive(true);
        ui.uiCraft.SetupCraftUI(storage);
        ui.player.input.Player.Attack.Disable();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        playerInventory = player.GetComponent<Inventory_Player>();
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        ui.player.input.Player.Attack.Enable();
        ui.uiStorage.gameObject.SetActive(false);
    }
}
