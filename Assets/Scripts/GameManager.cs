using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] GameObject portalPrefab;
    GameObject portalInstance;
    //public Coroutine ChangeSceneCo;

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
        {
            CreatePortal();
        }
    }

    private void CreatePortal()
    {
        if (SceneManager.GetActiveScene().name == "Level_0") return;

        if (portalInstance != null)
        {
            SaveManager.instance.GetGameData().portalPosition = Vector3.zero;
            Destroy(portalInstance.gameObject);
        }

        var pTransform = Player.playerTransform;
        pTransform.GetComponent<Entity_SFX>().PlayPortalCreate(.5f);
        portalInstance = Instantiate(portalPrefab, pTransform.position + new Vector3(pTransform.localScale.x * 2, 0, 0), Quaternion.identity);
    }

    public void RestartScene()
    {
        ChangeScene(SceneManager.GetActiveScene().name, RespawnType.Checkpoint);
    }

    public void ChangeScene(string nextScene, RespawnType spawnType, bool fadeOut = true)
    {
        SaveManager.instance.GetGameData().respawnScene = nextScene;
        SaveManager.instance.SaveGame();
        AudioManager.instance.ChangeMusic(SceneManager.GetActiveScene().name, nextScene, 1f);
        StartCoroutine(ChangeSceneCo(nextScene, spawnType, fadeOut));
    }

    IEnumerator ChangeSceneCo(string nextScene, RespawnType respawnType, bool fadeOut = true)
    {
        //Fade effect
        UI_FadeEffect fadeEffect;

        if (fadeOut)
        {
            fadeEffect = UI.instance.uiFade;
            fadeEffect.FadeOut(1);
            yield return fadeEffect.fadeCo;
        }

        SceneManager.LoadScene(nextScene);

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        fadeEffect = UI.instance.uiFade;
        fadeEffect.FadeIn(1);
        UI.instance.player.input.Enable();

        Vector3 position = GetSpawnPosition(respawnType);

        var gameData = SaveManager.instance.GetGameData();

        if (gameData.portalScene == SceneManager.GetActiveScene().name)
            Instantiate(portalPrefab, gameData.portalPosition, Quaternion.identity);

        if (position != Vector3.zero)
        {
            Player.playerTransform.position = position;
            FindAnyObjectByType<CinemachineCamera>().PreviousStateIsValid = false;
        }
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
            if (gameData.checkpointPosition != Vector3.zero &&
                gameData.respawnScene == gameData.checkpointScene)
                return gameData.checkpointPosition;
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
