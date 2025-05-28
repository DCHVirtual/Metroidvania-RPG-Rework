using UnityEngine;
using UnityEngine.UIElements;

public class SkillObject_TimeEcho : SkillObject
{
    [SerializeField] float wispMoveSpeed = 15;
    [SerializeField] GameObject onDeathVfx;
    private Skill_TimeEcho echoDetails;
    Entity_Health playerHealth;
    SkillObject_Health echoHealth;
    TrailRenderer wispTrail;
    Transform playerTransform;
    Player_SkillManager skillManager;
    Entity_StatusHandler statusHandler;

    int animTriggerCount = 0;
    int maxAttacks = 0;
    float multiplyChance;

    public void SetupEcho(Skill_TimeEcho echoDetails, int maxAttacks, float multiplyChance)
    {
        this.echoDetails = echoDetails;
        this.maxAttacks = maxAttacks;
        this.multiplyChance = multiplyChance;
        stats = echoDetails.player.stats;
        damageScale = echoDetails.damageScale;
        playerTransform = echoDetails.player.transform;
        playerHealth = echoDetails.player.health;
        skillManager = echoDetails.player.skillManager;
        statusHandler = echoDetails.player.statusHandler;
        
        echoHealth = GetComponent<SkillObject_Health>();
        wispTrail = GetComponentInChildren<TrailRenderer>();
        wispTrail.gameObject.SetActive(false);

        Invoke(nameof(HandleDeath), echoDetails.GetEchoDuration());
        FaceClosestTarget();
        HandleAttacks();
    }

    void HandleWispMovement()
    {
        transform.position = Vector2.MoveTowards(transform.position, 
            playerTransform.position, wispMoveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, playerTransform.position) < .3f)
        {
            HandleWispAbility();
            Destroy(gameObject);
        }
    }

    void HandleWispAbility()
    {
        float healAmount = echoHealth.damageTaken * echoDetails.GetPercentDamageHealed();
        playerHealth.Heal(healAmount);
        skillManager.ReduceAllCooldownsBy(echoDetails.GetCooldownReduction());
        if (echoDetails.ShouldRemoveNegativeEffects())
            statusHandler.StopAllStatusEffects();
    }

    void FaceClosestTarget()
    {
        var target = ClosestTarget();

        if (target == null) 
            return;

        int xDir = (int)transform.localScale.x;
        if (Mathf.Sign(target.position.x - transform.position.x) != xDir)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void Update()
    {
        if (rb.simulated)
        {
            anim.SetFloat("yVelocity", rb.linearVelocityY);
            rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
            HandleTriggers();
        }
        else
            HandleWispMovement();
    }

    public void PerformAttack()
    {
        DamageEnemiesInRadius(targetCheck, 1);

        if (targetGotHit == false)
            return;
        Debug.Log("Target got hit!");
        bool canMultiply = Random.value < multiplyChance;
        Debug.Log($"Can Multiply? {canMultiply}");
        if (canMultiply)
        {
            Vector3 spawnPoint = lastTarget.position;
            spawnPoint.x += (int)transform.localScale.x;
            echoDetails.CreateTimeEcho(spawnPoint);
        }
    }

    public void HandleDeath()
    {
        Instantiate(onDeathVfx, transform.position, Quaternion.identity);

        if (echoDetails.ShouldBeWisp())
            TurnIntoWisp();
        else
            Destroy(gameObject);
    }

    void TurnIntoWisp()
    {
        anim.gameObject.SetActive(false);
        wispTrail.gameObject.SetActive(true);
        rb.simulated = false;
    }

    public void CallAnimationTrigger()
    {
        animTriggerCount++;
    }

    void HandleTriggers()
    {
        if (maxAttacks > 0 && animTriggerCount == maxAttacks)
            HandleDeath();
    }

    void HandleAttacks()
    {
        if (maxAttacks == 0) return;

        if (maxAttacks == 3)
            anim.SetBool("UseCombo", true);
        else
            anim.SetInteger("Attack", Random.Range(1, 4));
    }
}
