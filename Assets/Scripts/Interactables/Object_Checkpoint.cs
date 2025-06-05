using System.Linq;
using UnityEngine;

public class Object_Checkpoint : MonoBehaviour
{
    Animator anim;
    Object_Checkpoint[] checkpoints;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        checkpoints = FindObjectsByType<Object_Checkpoint>(FindObjectsSortMode.None)
            .Where(p => p != this).ToArray();
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
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (var checkpoint in checkpoints)
            checkpoint.SetActive(false);
        
        SetActive(true);

        var data = SaveManager.instance.GetGameData();

        data.respawnPosition = transform.position;

        SaveManager.instance.SaveGame();
    }
}
