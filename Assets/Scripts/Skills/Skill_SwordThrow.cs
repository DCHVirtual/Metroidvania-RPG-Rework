using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Skill_SwordThrow : Skill
{
    SkillObject_Sword currentSword;

    [Header("Regular Sword Upgrade")]
    [SerializeField] GameObject swordPrefab;
    [Range(0, 10)]
    [SerializeField] float throwPower = 5;

    [Header("Pierce Sword Upgrade")]
    [SerializeField] GameObject swordPiercePrefab;
    public int pierceAmount = 2;

    [Header("Spin Sword Upgrade")]
    [SerializeField] GameObject swordSpinPrefab;
    public float attacksPerSecond = 3;
    public float autoReturnTimer = 5f;

    [Header("Bounce Sword Upgrade")]
    [SerializeField] GameObject swordBouncePrefab;
    public int bounceCount;
    public float bounceSpeed;

    [Header("Trajectory")]
    [SerializeField] GameObject dot;
    [SerializeField] int numDots;
    [SerializeField] float spaceBetweenDots = .05f;
    float swordGravity;
    Transform[] dots;
    Vector2 confirmedDirection;

    protected override void Awake()
    {
        base.Awake();
        swordGravity = swordPrefab.GetComponent<Rigidbody2D>().gravityScale;
        dots = GenerateDots();
    }

    public override bool CanUseSkill()
    {
        if (currentSword != null)
        {
            currentSword.SetReturnToPlayer();
            return false;
        }

        return base.CanUseSkill();
    }

    public void ThrowSword()
    {
        var newSword = Instantiate(GetSwordType(), player.transform.position, Quaternion.identity);
        currentSword = newSword.GetComponent<SkillObject_Sword>();
        currentSword.SetupSword(this, GetThrowPower());
    }

    GameObject GetSwordType()
    {
        if (Unlocked(SkillUpgradeType.SwordThrow_Pierce))
            return swordPiercePrefab;
        else if (Unlocked(SkillUpgradeType.SwordThrow_Spin))
            return swordSpinPrefab;
        else if (Unlocked(SkillUpgradeType.SwordThrow_Bounce)) 
            return swordBouncePrefab;

        return swordPrefab;
    }

    Vector2 GetThrowPower() => confirmedDirection * throwPower * 10f;

    public void PredictTrajectory(Vector2 dir)
    {
        for (int i = 0; i < numDots; i++)
        {
            dots[i].position = GetTrajectoryPoint(dir, i * spaceBetweenDots);
        }
    }

    Vector2 GetTrajectoryPoint(Vector2 dir, float t)
    {
        float scaledThrowPower = throwPower * 10;

        Vector2 initialVelocity = dir * scaledThrowPower;
        Vector2 gravityEffect = 0.5f * Physics2D.gravity * swordGravity * (t * t);
        Vector2 predictedPoint = (initialVelocity * t) + gravityEffect;
        Vector2 playerPos = transform.root.position;

        return playerPos + predictedPoint;
    }

    public void ConfirmTrajectory(Vector2 dir) => confirmedDirection = dir;
    public void EnableDots(bool enable)
    {
        foreach (var dot in dots)
            dot.gameObject.SetActive(enable);
    }
    Transform[] GenerateDots()
    {
        var newDots = new Transform[numDots];

        for (int i = 0; i < numDots; i++)
        {
            newDots[i] = Instantiate(dot, transform).transform;
            newDots[i].gameObject.SetActive(false);
        }

        return newDots;
    }
}
