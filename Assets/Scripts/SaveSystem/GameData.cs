using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public int gold;

    //Items, storage and equipment
    public SerializableDictionary<string, int> inventory;   //itemSaveID -> stackSize
    public SerializableDictionary<string, int> storageItems;
    public SerializableDictionary<string, int> storageMaterials;
    public List<string> equipment;

    //Skills
    public List<string> skillNames;
    public int skillPoints;

    //Checkpoints
    public Vector3 checkpointPosition;
    public string checkpointScene;

    //Town Portal
    public Vector3 portalPosition;
    public string portalScene;

    //Respawning
    public string respawnScene;

    public GameData()
    {
        inventory = new SerializableDictionary<string, int>();
        storageItems = new SerializableDictionary<string, int>();
        storageMaterials = new SerializableDictionary<string, int>();
        equipment = new List<string>();
        skillNames = new List<string>();
        checkpointPosition = Vector3.zero;
        respawnScene = "Level_0";
        portalPosition = Vector3.zero;
        portalScene = "";
        checkpointScene = "";
    }
}
