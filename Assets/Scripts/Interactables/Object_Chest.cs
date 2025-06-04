using UnityEngine;

public class Object_Chest : MonoBehaviour, IDamageable
{
    Animator anim => GetComponentInChildren<Animator>();
    Rigidbody2D rb => GetComponent<Rigidbody2D>();
    EntityFX fx => GetComponent<EntityFX>();
    Entity_DropManager dropManager => GetComponent<Entity_DropManager>();
    bool canDropItems = true;
    public bool TakeDamage(float dmg, Transform dmgDealer, 
        float elementalDmg = 0f, ElementType elemType = ElementType.None)
    {
        if (!canDropItems) return false;
        canDropItems = false;
        dropManager.DropItems();
        dropManager.DropItems();
        dropManager.DropItems();
        rb.linearVelocity = new Vector2(0, 5);
        rb.angularVelocity = Random.Range(-200f, 200f);
        fx.PlayDamageVFX();
        anim.SetBool("Open", true);

        return true;
    }
}
