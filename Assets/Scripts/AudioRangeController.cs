using UnityEngine;

public class AudioRangeController : MonoBehaviour
{
    AudioSource audioSource;
    float maxHearingRange = 15f;
    float maxVolume;
    Transform pTransform;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        maxVolume = audioSource.volume;
    }

    private void Start()
    {
        pTransform = Player.playerTransform;
    }

    private void Update()
    {
        float distToPlayer = Vector2.Distance(pTransform.position, transform.position);

        audioSource.volume = (1 - (distToPlayer / maxHearingRange)) * maxVolume;
    }
}
