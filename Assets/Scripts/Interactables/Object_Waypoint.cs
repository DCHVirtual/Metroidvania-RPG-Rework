using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Object_Waypoint : MonoBehaviour
{
    [field: SerializeField] public RespawnType waypointType { get; private set; }
    [field: SerializeField] public RespawnType connectedWaypointType { get; private set; }
    [SerializeField] string transferToScene;
    [SerializeField] Transform spawnPoint;
    bool canBeTriggered = true;

    private void OnValidate()
    {
        gameObject.name = $"Object_Waypoint - {waypointType} - {transferToScene}";

        if (waypointType == RespawnType.Enter)
            connectedWaypointType = RespawnType.Exit;
        else if (waypointType == RespawnType.Exit)
            connectedWaypointType = RespawnType.Enter;
    }

    public Vector3 GetSpawnPoint() => spawnPoint.position;

    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {

        //Zero respawnPosition to load at start of level instead of checkpoint
        SaveManager.instance.GetGameData().respawnPosition = Vector3.zero;
        SaveManager.instance.GetGameData().respawnScene = transferToScene;
        SaveManager.instance.SaveGame();
        GameManager.instance.ChangeScene(transferToScene, connectedWaypointType);

        yield return null;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canBeTriggered = true;
    }
}
