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
        ui.uiCraft.SetupCraftUI(storage);
        ui.uiCraft.gameObject.SetActive(true);
        ui.player.input.Player.Attack.Disable();
    }

    public bool CanInteract() => canInteract;

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        playerInventory = player.GetComponent<Inventory_Player>();
        canInteract = true;
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        ui.player.input.Player.Attack.Enable();
        ui.uiStorage.gameObject.SetActive(false);
        ui.uiCraft.gameObject.SetActive(false);
        canInteract = false;
    }
}
