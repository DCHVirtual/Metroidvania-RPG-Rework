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
    //[SerializeField] Color OnCritVFXColor = Color.white;

    [Header("Counter Attack VFX")]
    [SerializeField] GameObject CounterVFX;
    [SerializeField] Color CounterVFXColor = Color.white;

    [Header("Element Colors")]
    Color fireVFXcolor = new Color(1,165f/255, 0);
    Color iceVFXcolor = Color.cyan;
    Color lightningVFXcolor = Color.yellow;

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

    public Color ElementHitVFXColor(ElementType element)
    {
        Color color;
        switch (element)
        {
            case ElementType.Fire:
                color = fireVFXcolor;
                break;
            case ElementType.Ice:
                color = iceVFXcolor;
                break;
            case ElementType.Lightning:
                color = lightningVFXcolor;
                break;
            default:
                color = OnHitVFXColor;
                break;
        }
        return color;
    }

    public void PlayHitVFX(Vector2 position, bool isCrit, ElementType element)
    {
        var vfx = Instantiate(OnHitVFX, position, Quaternion.identity);
        Color vfxColor = ElementHitVFXColor(element);

        if (isCrit)
            vfx.transform.localScale *= 1.5f;

        vfx.GetComponentInChildren<SpriteRenderer>().color = vfxColor;
    }
    
    public void PlayCounterVFX(Vector2 position)
    {
        var vfx = Instantiate(OnHitVFX, position, Quaternion.identity);
        vfx.GetComponentInChildren<SpriteRenderer>().color = CounterVFXColor;
        vfx.transform.localScale *= 1.5f;
    }

    public void PlayStatusVFX(float duration, ElementType element)
    {
        StartCoroutine(PlayStatusVFX_Co(duration, ElementHitVFXColor(element)));
    }

    public void StopAllVFX()
    {
        StopAllCoroutines();
        sr.color = Color.white;
        sr.material = originalMat;
    }

    IEnumerator PlayStatusVFX_Co(float duration, Color color)
    {
        float tickInterval = .15f;
        float timePassed = 0f;

        Color lightColor = color * 1.5f;
        Color darkColor = color * .75f;
        Color[] colors = { lightColor, color, darkColor, color };
        int colorIndex = 1;

        while (timePassed < duration)
        {
            sr.color = colors[colorIndex++];
            if (colorIndex == colors.Length) colorIndex = 0;
            yield return new WaitForSeconds(tickInterval);
            timePassed += tickInterval;
        }

        sr.color = Color.white;
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
