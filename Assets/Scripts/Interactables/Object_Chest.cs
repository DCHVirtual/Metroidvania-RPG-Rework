using UnityEngine;

public class Object_Chest : MonoBehaviour, IDamageable
{
    Animator anim => GetComponentInChildren<Animator>();
    Rigidbody2D rb => GetComponent<Rigidbody2D>();
    EntityFX fx => GetComponent<EntityFX>();
    public bool TakeDamage(float dmg, Transform dmgDealer, 
        float elementalDmg = 0f, ElementType elemType = ElementType.None)
    {
        
        rb.linearVelocity = new Vector2(0, 5);
        rb.angularVelocity = Random.Range(-200f, 200f);
        fx.PlayDamageVFX();
        anim.SetBool("Open", true);

        return true;
    }
}
