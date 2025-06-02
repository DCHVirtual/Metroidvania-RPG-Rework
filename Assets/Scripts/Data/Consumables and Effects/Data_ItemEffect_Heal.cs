using UnityEngine;
[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Heal Effect", fileName = "Item Effect Data - Heal")]
public class Data_ItemEffect_Heal : Data_ItemEffectSO
{
    public float healPercent = .1f;

    public override string GetDescription()
    {
        return description.Replace(" N ", $" {healPercent * 100}% ");
    }

    public override void ExecuteEffect()
    {
        Player player = FindFirstObjectByType<Player>();

        float healAmount = player.stats.GetMaxHealth() * healPercent;

        player.health.Heal(healAmount);
    }
}
