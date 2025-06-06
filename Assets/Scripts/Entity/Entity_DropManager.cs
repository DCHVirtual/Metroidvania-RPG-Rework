using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Entity_DropManager : MonoBehaviour
{
    [SerializeField] GameObject itemDropPrefab;
    [SerializeField] Data_ItemListSO dropData;

    [Header("Drop Restrictions")]
    [SerializeField] int maxRarityAmount = 1200;
    [SerializeField] int maxItemsToDrop = 5;
    [Range(1.0f, 2.0f)][SerializeField] float dropChanceMultiplier;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
            DropItems();
    }
    public virtual void DropItems()
    {
        var itemsToDrop = RollDrops();
        int amountToDrop = Mathf.Min(itemsToDrop.Count, maxItemsToDrop);

        for (int i = 0; i < amountToDrop; i++)
            CreateItemDrop(itemsToDrop[i]);
    }

    protected void CreateItemDrop(Data_ItemSO itemToDrop)
    {
        GameObject newItem = Instantiate(itemDropPrefab, transform.position, Quaternion.identity);
        newItem.GetComponent<Object_ItemPickup>().SetupItem(itemToDrop);
    }

    List<Data_ItemSO> RollDrops()
    {
        var possibleDrops = new List<Data_ItemSO>();
        var finalDrops = new List<Data_ItemSO>();
        float maxRarityAmount = this.maxRarityAmount;

        foreach (var item in dropData.itemList)
        {
            if (Roll() <= item.dropChance)
                possibleDrops.Add(item);
        }

        possibleDrops.OrderByDescending(item => item.itemRarity).ToList();

        foreach (var item in possibleDrops)
        {
            if (maxRarityAmount >= item.itemRarity)
            {
                finalDrops.Add(item);
                maxRarityAmount -= item.itemRarity;
            }
        }

        return finalDrops;
    }

    public float Roll()
    {
        return Random.value * 100;
    }
}