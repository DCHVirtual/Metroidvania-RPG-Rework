using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_FadeEffect : MonoBehaviour
{
    Image img;
    public Coroutine fadeCo { get; private set; }

    private void Awake()
    {
        img = GetComponent<Image>();
    }

    public void FadeIn(float duration)
    {
        if (fadeCo != null)
            StopCoroutine(fadeCo);
        fadeCo = StartCoroutine(Fade(1, 0, duration));
    }

    public void FadeOut(float duration)
    {
        if (fadeCo != null)
            StopCoroutine(fadeCo);
        fadeCo = StartCoroutine (Fade(0, 1, duration));
    }

    IEnumerator Fade(float startAlpha, float targetAlpha, float duration)
    {
        float timePassed = 0f;
        float alpha = startAlpha;

        while (timePassed < duration)
        {
            alpha = Mathf.Lerp(startAlpha, targetAlpha, timePassed / duration);
            img.color = new Color(0, 0, 0, alpha);
            timePassed += Time.deltaTime;
            yield return null;
        }
    }
}
