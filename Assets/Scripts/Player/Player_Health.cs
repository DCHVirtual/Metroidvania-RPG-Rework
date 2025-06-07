using System.Collections;
using UnityEngine;

public class Player_Health : Entity_Health
{
    protected override void Die()
    {
        base.Die();

        //Trigger death UI
        StartCoroutine(Respawn());
    }

    private void Update()
    {
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2);
        GameManager.instance.RestartScene();
    }
}
