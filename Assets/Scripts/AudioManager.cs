using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] AudioClip townMusic;
    [SerializeField] AudioClip levelMusic;
    [SerializeField] List<AudioClip> audioList;
    Dictionary<string, AudioClip> audioDict;
    float maxMusicVolume;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        maxMusicVolume = bgmSource.volume;
    }

    private void Start()
    {
        audioDict = new Dictionary<string, AudioClip>();

        for (int i = 0; i < audioList.Count; i++)
            audioDict[audioList[i].name] = audioList[i];

        if (SceneManager.GetActiveScene().name == "Level_0")
            bgmSource.clip = townMusic;
        else
            bgmSource.clip = levelMusic;

        bgmSource.volume = 0;
        bgmSource.Play();

        FadeIn();
    }

    public void ChangeMusic(string currentScene, string nextScene, float fadeDuration)
    {
        if (currentScene == "Level_0")
            StartCoroutine(ChangeMusicCo(levelMusic, fadeDuration));
        else if (nextScene == "Level_0")
            StartCoroutine(ChangeMusicCo(townMusic, fadeDuration));
    }

    IEnumerator ChangeMusicCo(AudioClip nextClip, float fadeDuration)
    {
        yield return StartCoroutine(FadeCo(maxMusicVolume, 0, fadeDuration));
        bgmSource.Stop();
        bgmSource.clip = nextClip;
        bgmSource.Play();
        StartCoroutine(FadeCo(0, maxMusicVolume, fadeDuration));

    }

    void FadeIn()
    {
        StartCoroutine(FadeCo(0, maxMusicVolume, 1));
    }
    void FadeOut()
    {
        StartCoroutine(FadeCo(maxMusicVolume, 0, 1));
    }

    IEnumerator FadeCo(float startVolume, float targetVolume, float duration)
    {
        float timePassed = 0;

        while (timePassed < duration)
        {
            bgmSource.volume = Mathf.Lerp(startVolume, targetVolume, timePassed/duration);
            timePassed += Time.deltaTime;
            yield return null;
        }
    }

    public void PlaySFX(string soundName, AudioSource sfxSource, float volume = 1)
    {
        var clip = audioDict[soundName];

        sfxSource.clip = clip;
        sfxSource.volume = volume;
        sfxSource.pitch = Random.Range(0.8f, 1.2f);
        sfxSource.PlayOneShot(clip);
    }

    public void Play3DSFX(string soundName, AudioSource sfxSource, float volume)
    {

    }
}
