using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Winter's Touch", fileName = "Item Effect Data - Winters Touch")]

public class Data_ItemEffect_WintersTouch : Data_ItemEffectSO
{
    [SerializeField] GameObject iceBlastVfx;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] Data_Elemental effectData;
    [SerializeField] float healthPercentTrigger = .25f;
    [SerializeField] float iceDamage = 150;
    [SerializeField] float cooldown;
    float lastTimeUsed =  -float.MaxValue;

    public override void ExecuteEffect()
    {

        bool noCooldown = Time.time >= lastTimeUsed + cooldown;
        bool reachedThreshold = player.health.GetHealthPercent() <= healthPercentTrigger;

        Debug.Log($"Last Time Used: {lastTimeUsed}\nCooldown? {!noCooldown} Reached threshold? {reachedThreshold}");

        if (noCooldown && reachedThreshold)
        {
            player.fx.CreateVFX(iceBlastVfx, player.transform);
            DamageEnemiesInRadius();
            lastTimeUsed = Time.time;
        }
    }

    private void OnEnable()
    {
        lastTimeUsed = -float.MaxValue;
    }

    public void DamageEnemiesInRadius()
    {
        var enemies = Physics2D.OverlapCircleAll(player.transform.position, 5, enemyLayer)
            .Where(hit => hit.GetComponent<IDamageable>() != null).ToList();

        foreach (var target in enemies)
        {
            var damageable = target.GetComponent<IDamageable>();
            bool targetGotHit = damageable.TakeDamage(0, player.transform, 150, ElementType.Ice);
            
            if (targetGotHit)
            {
                var vfxPos = (Vector2)target.transform.position + Random.insideUnitCircle * .5f;
                player.fx.PlayHitVFX(vfxPos, false, ElementType.Ice);
                target.GetComponent<Entity_StatusHandler>()?.ApplyStatusEffect(ElementType.Ice, effectData);
            }
        }
    }

    public override void Subscribe(Player player)
    {
        base.Subscribe(player);
        player.health.OnTakingDamage += ExecuteEffect;
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        player.health.OnTakingDamage -= ExecuteEffect;
        player = null;
    }
}
