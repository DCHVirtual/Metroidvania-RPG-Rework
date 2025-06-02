using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Buff Effect", fileName = "Item Effect Data - Buff")]
public class Data_ItemEffect_Buff : Data_ItemEffectSO
{
    [SerializeField] Buff[] buffs;
    [SerializeField] float duration;
    [SerializeField] string source;

    Data_BuffEffect buffEffect;

    Player_Stats playerStats;

    private void Awake()
    {
        
    }

    public override bool CanBeUsed()
    {
        playerStats = FindFirstObjectByType<Player_Stats>();
        return playerStats.CanApplyBuff(buffEffect.source);
    }
    public override void ExecuteEffect()
    {
        buffEffect = new Data_BuffEffect(buffs, duration, source);
        playerStats.ApplyBuff(buffEffect);
    }
}
