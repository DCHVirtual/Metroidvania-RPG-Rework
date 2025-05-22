using UnityEngine;

public class Player_Combat : Entity_Combat
{
    [field: Header("Counter Details")]
    [field: SerializeField] public float counterLockDuration { get; private set; }

    public bool AttemptCounterAttack()
    {
        int counteredEnemies = 0;

        foreach (var target in GetDetectedColliders())
        {
            var enemy = target.GetComponent<Enemy>();
            if (enemy && enemy.DetectCounter())
            {
                counteredEnemies++;
                GetComponent<EntityFX>().PlayCounterVFX(target.transform.position);
            }
        }

        return counteredEnemies > 0;
    }
}
