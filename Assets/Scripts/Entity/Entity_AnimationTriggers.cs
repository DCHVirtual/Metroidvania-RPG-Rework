using UnityEngine;

public class Entity_AnimationTriggers : MonoBehaviour
{
    Entity_SFX sfx;

    private void Start()
    {
        sfx = GetComponentInParent<Entity_SFX>();
    }
    public void PlaySwingSFX()
    {
        sfx.PlayAttackSwing(.5f);
    }
    public void PlayFootstepSFX()
    {
        sfx.PlayFootstep(1f);
    }
}
