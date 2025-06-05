using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] GameObject portalPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            Instantiate(portalPrefab, Player.playerTransform.position + new Vector3(2,0,0), Quaternion.identity);
    }

    public void RestartScene()
    {
        SaveManager.instance.SaveGame();

        ChangeScene(SceneManager.GetActiveScene().name, RespawnType.Checkpoint);
    }

    public void ChangeScene(string sceneName, RespawnType spawnType)
    {
        StartCoroutine(ChangeSceneCo(sceneName, spawnType));
    }

    IEnumerator ChangeSceneCo(string sceneName, RespawnType respawnType)
    {
        //Fade effect

        //yield return new WaitForSeconds(1);
        yield return new WaitForEndOfFrame();

        SceneManager.LoadScene(sceneName);

        //yield return new WaitForSeconds(0.2f);
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();


        Vector3 position = GetSpawnPosition(respawnType);

        var gameData = SaveManager.instance.GetGameData();
        SaveManager.instance.SaveGame();

        if (gameData.portalScene == SceneManager.GetActiveScene().name)
            Instantiate(portalPrefab, gameData.portalPosition, Quaternion.identity);

        if (position != Vector3.zero)
            Player.playerTransform.position = position;
    }

    Vector3 GetSpawnPosition(RespawnType type)
    {
        if (type == RespawnType.Enter || type == RespawnType.Exit)
        {
            var waypoints = FindObjectsByType<Object_Waypoint>(FindObjectsSortMode.None);

            foreach (var waypoint in waypoints)
            {
                if (waypoint.waypointType == type)
                    return waypoint.GetSpawnPoint();
            }
        }

        var gameData = SaveManager.instance.GetGameData();

        if (type == RespawnType.Checkpoint)
        {
            if (gameData.respawnPosition != Vector3.zero)
                return gameData.respawnPosition;
        }
        else if (type == RespawnType.PortalFromTown)
        {
            var portalPosition = gameData.portalPosition;

            //After portal is used, it is no longer usable, get rid of its stuff
            gameData.portalPosition = Vector3.zero;
            gameData.portalScene = "";
            return portalPosition;
        }

        return Vector3.zero;
    }
}
