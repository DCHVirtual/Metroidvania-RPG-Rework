using System;
using System.Collections.Generic;

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

    public GameData()
    {
        inventory = new SerializableDictionary<string, int>();
        storageItems = new SerializableDictionary<string, int>();
        storageMaterials = new SerializableDictionary<string, int>();
        equipment = new List<string>();
        skillNames = new List<string>();
    }
}
