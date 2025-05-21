using UnityEngine;

public class Chest : MonoBehaviour, IDamageable
{
    Animator anim => GetComponentInChildren<Animator>();
    Rigidbody2D rb => GetComponent<Rigidbody2D>();
    EntityFX fx => GetComponent<EntityFX>();
    public void TakeDamage(float dmg, Transform dmgDealer)
    {
        
        rb.linearVelocity = new Vector2(0, 5);
        rb.angularVelocity = Random.Range(-200f, 200f);
        fx.PlayDamageVFX();
        anim.SetBool("Open", true);
    }
}
