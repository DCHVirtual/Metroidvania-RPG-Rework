using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Object_Portal : MonoBehaviour
{
    [SerializeField] RespawnType spawnType;

    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();           //Waiting for SaveManager to instantiate GameData
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        var gameData = SaveManager.instance.GetGameData();
        var portalPosition = gameData.portalPosition;

        if (spawnType == RespawnType.PortalFromLevel)   //Portal created in level
        {   
            if (portalPosition != Vector3.zero)         //Portal from previous save
                transform.position = portalPosition;
            else                                        //New portal
            {
                gameData.portalPosition = transform.position;
                gameData.portalScene = SceneManager.GetActiveScene().name;
            }
        }
        else if (portalPosition == Vector3.zero)        //Portal in town only active if portal in level exists
            gameObject.SetActive(false);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var portalScene = SaveManager.instance.GetGameData().portalScene;

        UI.instance.player.input.Disable();

        if (spawnType == RespawnType.PortalFromLevel)
            GameManager.instance.ChangeScene("Level_0", RespawnType.PortalFromLevel);
        else
            GameManager.instance.ChangeScene(portalScene, RespawnType.PortalFromTown);
    }
}