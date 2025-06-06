using UnityEngine;

public class Entity_SFX : MonoBehaviour
{
    public AudioSource audioSource { get; private set; }

    private void Awake()
    {
        audioSource = GetComponentInChildren<AudioSource>();
    }
    public void PlayAttackHit(float volume = 1)
    {
        AudioManager.instance.PlaySFX("SFX_BasicAttack", audioSource, volume);
    }
    public void PlayAttackSwing(float volume = 1)
    {
        AudioManager.instance.PlaySFX("SFX_SwordSwing", audioSource, volume);
    }
    public void PlayChestOpen(float volume = 1)
    {
        AudioManager.instance.PlaySFX("SFX_ChestOpen", audioSource, volume);
    }
    public void PlayCounter(float volume = 1)
    {
        AudioManager.instance.PlaySFX("SFX_Counter", audioSource, volume);
    }
    public void PlayDash(float volume = 1)
    {
        AudioManager.instance.PlaySFX("SFX_Dash", audioSource, volume);
    }
    public void PlayDomain(float volume = 1)
    {
        AudioManager.instance.PlaySFX("SFX_Domain", audioSource, volume);
    }
    public void PlayFire(float volume = 1)
    {
        AudioManager.instance.PlaySFX("SFX_Fire", audioSource, volume);
    }
    public void PlayInventoryOpen(float volume = 1)
    {
        AudioManager.instance.PlaySFX("SFX_InventoryOpen", audioSource, volume);
    }
    public void PlayJump(float volume = 1)
    {
        AudioManager.instance.PlaySFX("SFX_Jump", audioSource, volume);
    }
    public void PlayLevelChange(float volume = 1)
    {
        AudioManager.instance.PlaySFX("SFX_LevelChange", audioSource, volume);
    }
    public void PlayShardExplode(float volume = 1)
    {
        AudioManager.instance.PlaySFX("SFX_ShardExplode", audioSource, volume);
    }
    public void PlaySwordThrow(float volume = 1)
    {
        AudioManager.instance.PlaySFX("SFX_SwordThrow", audioSource, volume);
    }
    public void PlayItemPickup(float volume = 1)
    {
        AudioManager.instance.PlaySFX("SFX_ItemPickup", audioSource, volume);
    }
    public void PlayPortalEnter(float volume = 1)
    {
        AudioManager.instance.PlaySFX("SFX_PortalEnter", audioSource, volume);
    }
    public void PlayPortalCreate(float volume = 1)
    {
        AudioManager.instance.PlaySFX("SFX_PortalCreate", audioSource, volume);
    }
    public void PlayEquip(float volume = 1)
    {
        AudioManager.instance.PlaySFX("SFX_Equip", audioSource, volume);
    }
    public void PlayStorageTransfer(float volume = 1)
    {
        AudioManager.instance.PlaySFX("SFX_StorageTransfer", audioSource, volume);
    }
    public void PlayNPCInteract(float volume = 1)
    {
        AudioManager.instance.PlaySFX("SFX_NPCInteract", audioSource, volume);
    }
    public void PlayFootstep(float volume = 1)
    {
        AudioManager.instance.PlaySFX("SFX_Footstep", audioSource, volume);
    }
    public void PlayJumpLand(float volume = 1)
    {
        AudioManager.instance.PlaySFX("SFX_JumpLand", audioSource, volume);
    }

}
