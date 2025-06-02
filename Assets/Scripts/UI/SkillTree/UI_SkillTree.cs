using UnityEngine;

public class UI_SkillTree : MonoBehaviour
{
    public int skillPoints;
    [SerializeField] UI_NodeConnectHandler[] parentNodes;
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
}
