using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Object_Portal : MonoBehaviour
{
    [SerializeField] RespawnType spawnType;

    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        var gameData = SaveManager.instance.GetGameData();
        var portalPosition = gameData.portalPosition;
        if (spawnType == RespawnType.PortalFromLevel)
        {
            if (portalPosition != Vector3.zero)
                transform.position = portalPosition;
            else
            {
                gameData.portalPosition = transform.position;
                gameData.portalScene = SceneManager.GetActiveScene().name;
            }
        }
        else if (portalPosition == Vector3.zero)
            gameObject.SetActive(false);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var save = SaveManager.instance;
        var data = save.GetGameData();

        if (spawnType == RespawnType.PortalFromLevel)
        {
            GetComponent<Collider2D>().isTrigger = false;
            data.respawnScene = "Level_0";
            save.SaveGame();
            GameManager.instance.ChangeScene("Level_0", RespawnType.PortalFromLevel);
        }
        else
        {
            data.respawnScene = data.portalScene;
            save.SaveGame();
            GameManager.instance.ChangeScene(data.portalScene, RespawnType.PortalFromTown);
        }
    }
}
