using UnityEngine;

public class NPC_Merchant : NPC, IInteractable
{
    Inventory_Player playerInventory;
    Inventory_Merchant merchant;

    protected override void Awake()
    {
        base.Awake();
        anim.SetBool("Merchant", true);
        merchant = GetComponent<Inventory_Merchant>();
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Z)) {
            merchant.FillShopList();
        }
    }

    public void Interact()
    {
        ui.uiMerchant.SetupMerchantUI(merchant, playerInventory);
        ui.uiMerchant.gameObject.SetActive(true);
        ui.player.input.Player.Attack.Disable();
    }

    public bool CanInteract() => canInteract;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        playerInventory = player.GetComponent<Inventory_Player>();
        merchant.SetInventory(playerInventory);
        canInteract = true;
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        ui.SwitchOffAllTooltips();
        ui.uiMerchant.gameObject.SetActive(false);
        ui.player.input.Player.Attack.Enable();
        canInteract = false;
    }
}
