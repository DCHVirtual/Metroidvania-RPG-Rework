using UnityEngine;

public class SkillObject_SwordPierce : SkillObject_Sword
{
    private int pierceAmount;

    public override void SetupSword(Skill_SwordThrow swordDetails, Vector2 forceDir)
    {
        base.SetupSword(swordDetails, forceDir);
        pierceAmount = swordDetails.pierceAmount;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        bool groundHit = collision.gameObject.layer == LayerMask.NameToLayer("Ground");

        if (pierceAmount == 0 || groundHit)
            StopSword(collision);

        pierceAmount--;

        DamageEnemy(collision);
    }
}
