using UnityEngine;

public class SkillObject_Domain : SkillObject
{
    Skill_Domain domainDetails;
    float expandSpeed = 2;
    float slowDownPercent = .9f;
    float duration;
    bool isShrinking;

    Vector3 targetScale;

    

    public void SetupDomain( Skill_Domain domainDetails)
    {
        this.domainDetails = domainDetails;

        float maxSize = domainDetails.maxDomainSize;
        slowDownPercent = domainDetails.GetSlowPercent();
        duration = domainDetails.GetDuration();
        expandSpeed = domainDetails.expandSpeed;
        targetScale = Vector3.one * maxSize;

        Invoke(nameof(ShrinkDomain), duration);
    }

    private void Update()
    {
        HandleScaling();
    }

    void HandleScaling()
    {
        float sizeDifference = Mathf.Abs(transform.localScale.x - targetScale.x);
        bool shouldChangeScale = sizeDifference > .1f;

        if (shouldChangeScale)
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, expandSpeed * Time.deltaTime);

        if (isShrinking && sizeDifference < .1f)
        {
            domainDetails.ClearTargets();
            Destroy(gameObject);
        }
    }

    void ShrinkDomain()
    {
        targetScale = Vector3.zero;
        isShrinking = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy == null)
            return;

        enemy.SlowEntity(duration, 1 - slowDownPercent, true);
        domainDetails.AddTarget(enemy);
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
