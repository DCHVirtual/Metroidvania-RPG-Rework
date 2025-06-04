using System.Collections.Generic;
using UnityEngine;

public class UI_SkillTree : MonoBehaviour, ISaveable
{
    public int skillPoints;
    [SerializeField] UI_NodeConnectHandler[] parentNodes;
    List<SkillData_SO> unlockedNodes = new List<SkillData_SO>();
    public Player_SkillManager skillManager { get; private set; }

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
            node.UpdateAllConnections();
        }
    }

    public void ApplyRedToDisabledConnex()
    {
        foreach (var node in parentNodes)
        {
            node.SetDisabledConnection();
        }
    }

    public void GetUnlockedNodes(UI_NodeConnectHandler node)
    {
        if (node.treeNode.isUnlocked)
        {
            unlockedNodes.Add(node.treeNode.skillData);
        }
    }

    public void LoadData(GameData data)
    {
        throw new System.NotImplementedException();
    }

    public void SaveData(ref GameData data)
    {
        throw new System.NotImplementedException();
    }
}
