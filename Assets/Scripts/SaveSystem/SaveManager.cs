using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    public bool encryptData;
    FileDataHandler dataHandler;
    GameData gameData;
    List<ISaveable> allSaveables;
    [SerializeField] string fileName = "2D_Souls.json";


    private void Awake()
    {
        instance = this;
    }

    public ref GameData GetGameData() => ref gameData;

    private IEnumerator Start()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        allSaveables = FindISaveables();

        yield return new WaitForEndOfFrame();

        LoadGame();
    }

    [ContextMenu("Save Game")]
    public void SaveGame()
    {
        DeleteSaveData();

        foreach (var saveable in allSaveables)
            saveable.SaveData(ref gameData);

        dataHandler.SaveData(gameData);
    }

    public void LoadGame()
    {
        gameData = dataHandler.LoadData();

        if (gameData == null)
        {
            Debug.Log("No save data found, creating new save file");
            gameData = new GameData();
            UI.instance.uiFade.FadeIn(1f);
            return;
        }

        LoadAllData();

        if (gameData.respawnScene != SceneManager.GetActiveScene().name)
            GameManager.instance.ChangeScene(gameData.respawnScene, RespawnType.Checkpoint, false);
        else
            UI.instance.uiFade.FadeIn(1f);
    }

    public void LoadAllData()
    {
        foreach (var saveable in allSaveables)
            saveable.LoadData(gameData);
    }

    [ContextMenu("*** Delete Save Data ***")]
    public void DeleteSaveData()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        dataHandler.DeleteSaveData();
    }

    /*void OnApplicationQuit()
    {
        SaveGame();
    }*/

    List<ISaveable> FindISaveables()
    {
        return
            FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None)
            .OfType<ISaveable>().ToList();
    }
}
