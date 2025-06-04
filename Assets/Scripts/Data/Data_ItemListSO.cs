using System.Linq;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item List", fileName = "List of Items - ")]

public class Data_ItemListSO : ScriptableObject
{
    public Data_ItemSO[] itemList;

    public Data_ItemSO GetItemData(string saveID)
    {
        return itemList.FirstOrDefault(item => item != null && item.saveID == saveID);
    }

#if UNITY_EDITOR
    [ContextMenu("Auto-fill with all ItemDataSO")]
    public void CollectItemsData()
    {
        string[] guids = AssetDatabase.FindAssets("t:Data_ItemSO");
        itemList =
            guids.Select(guid => AssetDatabase.LoadAssetAtPath<Data_ItemSO>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(item => item != null).ToArray();
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }
#endif
}
