using System.Collections.Generic;
using UnityEngine;

public class UI_SkillTree : MonoBehaviour, ISaveable
{
    public int skillPoints;
    [SerializeField] UI_NodeConnectHandler[] parentNodes;
    public Player_SkillManager skillManager { get; private set; }
    int test = 0, test1 = 0;
    public bool HaveEnoughSkillPoints(int cost) => skillPoints >= cost;
    public void RemoveSkillPoints(int cost) => skillPoints -= cost;
    public void AddSkillPoints(int points) => skillPoints += points;
    private void Awake()
    {
        skillManager = FindAnyObjectByType<Player_SkillManager>();
    }
    private void Start()
    {
        UpdateAllConnections();
    }

    [ContextMenu("Update All Connections")]
    public void UpdateAllConnections()
    {
        foreach (var node in parentNodes)
        {
            node.SetStartConnexColor();
            node.UpdateAllConnections();
        }
    }

    public void SetStartConnexColors()
    {
        foreach (var node in parentNodes)
        {
            node.SetStartConnexColor();
        }
    }
    
    public void ApplyRedToDisabledConnex()
    {
        foreach (var node in parentNodes)
        {
            node.SetDisabledConnection();
        }
    }

    #region Save / Load Functions
    public List<string> GetAllUnlockedNodeNames()
    {
        List<string> unlockedNodeNames = new List<string>();

        foreach (var node in parentNodes)
            GetUnlockedNodeNames(node, unlockedNodeNames);

        return unlockedNodeNames;
    }

    public void GetUnlockedNodeNames(UI_NodeConnectHandler node, List<string> unlockedNodeNames)
    {
        if (node.treeNode.isUnlocked)
        {
            unlockedNodeNames.Add(node.treeNode.skillData.displayName);
            foreach (var detail in node.connectDetails)
            {
                if (detail.childNode != null)
                    GetUnlockedNodeNames(detail.childNode, unlockedNodeNames);
            }
        }
    }

    public void LoadData(GameData data)
    {
        gameObject.SetActive(true);

        skillPoints = data.skillPoints;

        foreach (var node in parentNodes)
            LoadNode(data, node);

        gameObject.SetActive(false);
    }

    public void LoadNode(GameData data, UI_NodeConnectHandler node)
    {
        if (data.skillNames.Contains(node.treeNode.skillData.displayName))
        {
            node.treeNode.UnlockSkillFromSave();

            foreach (var detail in node.connectDetails)
            {
                if (detail.childNode != null)
                    LoadNode(data, detail.childNode);
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        data.skillNames = GetAllUnlockedNodeNames();
        data.skillPoints = skillPoints;
    }
    #endregion
}
