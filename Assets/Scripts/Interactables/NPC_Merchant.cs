using UnityEngine;

public class NPC_Merchant : NPC, IInteractable
{
    protected override void Awake()
    {
        base.Awake();
        anim.SetBool("Merchant", true);
    }

    public void Interact()
    {
        Debug.Log("Open Merchant Shop");
    }
}
