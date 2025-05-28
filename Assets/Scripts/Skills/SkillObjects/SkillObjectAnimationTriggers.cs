using UnityEngine;

public class SkillObjectAnimationTriggers : MonoBehaviour
{
    SkillObject_TimeEcho timeEcho;
    private void Awake()
    {
        timeEcho = GetComponentInParent<SkillObject_TimeEcho>();
    }

    public void AnimationTrigger()
    {
        timeEcho.CallAnimationTrigger();
    }

    public void AttackTrigger()
    {
        timeEcho.PerformAttack();
    }
}
