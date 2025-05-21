using System.Collections;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    float flashTime = 0.2f;
    [Header("Damage Received Material")]
    [SerializeField] Material OnDmgVFX;

    [Header("Damage Dealt VFX")]
    [SerializeField] GameObject OnHitVFX;
    [SerializeField] Color OnHitVFXColor = Color.white;

    [Header("Counter Attack VFX")]
    [SerializeField] GameObject CounterVFX;
    [SerializeField] Color CounterVFXColor = Color.white;

    Material originalMat;
    SpriteRenderer sr;
    Entity entity;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        entity = GetComponent<Entity>();
    }

    public void Start()
    {
        originalMat = sr.material;
    }

    public void PlayDamageVFX()
    {
        StartCoroutine(FlashFX());
    }

    public void PlayHitVFX(Vector2 position)
    {
        var vfx = Instantiate(OnHitVFX, position, Quaternion.identity);
        vfx.GetComponentInChildren<SpriteRenderer>().color = OnHitVFXColor;
    }
    
    public void PlayCounterVFX(Vector2 position)
    {
        var vfx = Instantiate(OnHitVFX, position, Quaternion.identity);
        vfx.GetComponentInChildren<SpriteRenderer>().color = CounterVFXColor;
        vfx.transform.localScale *= 1.5f;
    }

    IEnumerator FlashFX()
    {
        sr.material = OnDmgVFX;
        yield return new WaitForSecondsRealtime(flashTime);
        sr.material = originalMat;
    }

    void BlinkRed()
    {
        if (sr.color != Color.white)
            sr.color = Color.white;
        else
            sr.color = Color.red;
    }

    void CancelBlink()
    {
        CancelInvoke();
        sr.color = Color.white;
    }
}
