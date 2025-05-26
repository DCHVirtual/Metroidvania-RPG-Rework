using System.Collections;
using UnityEngine;

public class PlayerFX : EntityFX
{
    [Header("Image Echo VFX")]
    [Range(.01f, .2f)]
    [SerializeField] float afterImageInterval;
    [SerializeField] GameObject afterImagePrefab;
    Coroutine afterImageCo;

    public void AfterImageEffect(float duration)
    {
        if (afterImageCo != null)
            StopCoroutine(afterImageCo);
        StartCoroutine(AfterImageCo(duration));
    }

    IEnumerator AfterImageCo(float duration)
    {
        float time = 0;

        while (time < duration)
        {
            CreateImageEcho();
            yield return new WaitForSeconds(afterImageInterval);
            time += afterImageInterval;
        }
    }

    void CreateImageEcho()
    {
        GameObject afterImage = Instantiate(afterImagePrefab, transform.position, Quaternion.identity);
        afterImage.transform.localScale = transform.localScale;
        afterImage.GetComponentInChildren<SpriteRenderer>().sprite = sr.sprite;
    }
}
