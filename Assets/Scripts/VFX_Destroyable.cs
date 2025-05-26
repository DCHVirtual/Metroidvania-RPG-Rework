using System.Collections;
using UnityEngine;

public class VFX_Destroyable : MonoBehaviour
{
    SpriteRenderer sr;
    [SerializeField] float destroyTime = 1f;
    [SerializeField] float fadeSpeed = 1f;
    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }
    private void Start()
    {
        if (fadeSpeed > 0)
            StartCoroutine(FadeCo());

        if (destroyTime > 0)
            Destroy(gameObject, destroyTime);
    }

    IEnumerator FadeCo()
    {
        Color targetColor = Color.white;

        while(targetColor.a > 0)
        {
            targetColor.a = targetColor.a - (fadeSpeed * Time.deltaTime);
            sr.color = targetColor;
            yield return null;
        }
    }
}
