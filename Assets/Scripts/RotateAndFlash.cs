using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class RotateAndFlash : MonoBehaviour
{
    SpriteRenderer sr;
    float colorChangeSpeed = 0.175f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.position, Vector3.forward, .5f);
    }

    void OnEnable()
    {
        StartCoroutine(TurnWhite());
    }

    IEnumerator TurnWhite()
    {
        while (sr.color.g < 1f)
        {
            sr.color = new Color(sr.color.r, sr.color.g + colorChangeSpeed, sr.color.b + colorChangeSpeed);
            yield return null;
        }
        StartCoroutine(TurnRed());
    }
    IEnumerator TurnRed()
    {
        while (sr.color.g > 0f)
        {
            sr.color = new Color(sr.color.r, sr.color.g - colorChangeSpeed, sr.color.b - colorChangeSpeed);
            yield return null;
        }
        StartCoroutine(TurnWhite());
    }
}
