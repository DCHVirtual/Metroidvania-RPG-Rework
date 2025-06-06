using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Object_Checkpoint : MonoBehaviour
{
    Animator anim;
    Object_Checkpoint[] checkpoints;
    AudioSource audioSource;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        checkpoints = FindObjectsByType<Object_Checkpoint>(FindObjectsSortMode.None)
            .Where(p => p != this).ToArray();
        audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
    }

    public void LoadData(GameData data)
    {
    }

    public void SaveData(ref GameData data)
    {
        
    }

    public void SetActive(bool active)
    {
        anim.SetBool("Active", active);

        if (!active)
            audioSource.Stop();
        else if (active && !audioSource.isPlaying) 
        {
            audioSource.Play();
        }
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (var checkpoint in checkpoints)
            checkpoint.SetActive(false);
        
        SetActive(true);

        var data = SaveManager.instance.GetGameData();

        data.checkpointPosition = transform.position;
        data.respawnScene = SceneManager.GetActiveScene().name;
        data.checkpointScene = SceneManager.GetActiveScene().name;

        SaveManager.instance.SaveGame();
    }
}
