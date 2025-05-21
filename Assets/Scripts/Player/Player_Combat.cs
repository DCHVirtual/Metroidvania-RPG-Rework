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
            var counterable = target.GetComponent<ICounterable>();
            if (counterable == null)
                continue;

            if (counterable.WasCountered())
            {
                counteredEnemies++;
                GetComponent<EntityFX>().PlayCounterVFX(target.transform.position);
            }
        }

        Debug.Log($"Countered enemies: {counteredEnemies}");
        return counteredEnemies > 0;
    }
}
