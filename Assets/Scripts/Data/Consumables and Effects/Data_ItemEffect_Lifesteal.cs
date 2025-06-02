using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Lifesteal Effect", fileName = "Item Effect Data - Lifesteal")]
public class Data_ItemEffect_Lifesteal : Data_ItemEffectSO
{
    [SerializeField] float lifestealPercent = .15f;

    public override void Subscribe(Player player)
    {
        base.Subscribe(player);
        player.combat.OnDamageDealt += Lifesteal;
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        player.combat.OnDamageDealt -= Lifesteal;
        player = null;
    }

    void Lifesteal(float damage)
    {
        player.health.Heal(damage * lifestealPercent);
    }
}
