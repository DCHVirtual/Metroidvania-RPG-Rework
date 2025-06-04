using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    UI ui;
    RectTransform rect;
    UI_SkillTree skillTree;
    public UI_NodeConnectHandler connectHandler { get; private set; }
    Player_SkillManager skillManager;

    [Header("Unlock details")]
    public UI_TreeNode[] neededNodes;
    public UI_TreeNode[] conflictNodes;
    public bool isUnlocked;
    public bool isDisabled;

    [field: Header("Skill details")]
    [field: SerializeField] public SkillData_SO skillData { get; private set; }
    [SerializeField] string skillName;
    [SerializeField] Image skillIcon;
    [SerializeField] Image skillBorder;
    [SerializeField] Color skillLockedColor;
    [SerializeField] Color skillBorderColor;

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
        skillTree = GetComponentInParent<UI_SkillTree>();
        connectHandler = GetComponent<UI_NodeConnectHandler>();
        skillManager = FindFirstObjectByType<Player_SkillManager>();
        UpdateIconColor(skillLockedColor);
        
    }

    private void Start()
    {
        if (skillData.unlockedByDefault)
            UnlockSkill();
    }

    void UnlockSkill()
    {
        isUnlocked = true;
        UpdateIconColor(Color.white);
        DisableConflictNodes(true);

        skillTree.RemoveSkillPoints(skillData.cost);
        connectHandler.UnlockConnectionImage(true);
        skillTree.ApplyRedToDisabledConnex();

        Skill skill = skillTree.skillManager.GetSkillByType(skillData.skillType);
        skill.SetSkillUpgrade(skillData);
        skill.ResetCooldown();
    }

    public void RefundSkill()
    {
        if (isUnlocked == false || skillData.unlockedByDefault) return;

        isUnlocked = false;
        isDisabled = false;
        UpdateIconColor(skillLockedColor);
        DisableConflictNodes(false);

        skillManager.GetSkillByType(skillData.skillType).ResetSkill();

        skillTree.AddSkillPoints(skillData.cost);
        connectHandler.UnlockConnectionImage(false);
    }

    bool CanBeUnlocked()
    {
        if (isDisabled || isUnlocked)
            return false;

        if (!skillTree.HaveEnoughSkillPoints(skillData.cost))
            return false;

        foreach (var node in neededNodes)
            if (!node.isUnlocked) return false;

        foreach (var node in conflictNodes)
            if (node.isUnlocked) return false;

        return true;
    }

    void DisableConflictNodes(bool disable)
    {
        foreach (var node in conflictNodes) 
        {
            node.DisableNode(disable);
        }
    }

    public void DisableNode(bool disable)
    {
        isDisabled = disable;
        
        foreach(var detail in connectHandler.connectDetails)
        {
            if (detail.childNode != null)
            {
                detail.childNode.treeNode.DisableNode(disable);
            }
        }
    }

    void UpdateIconColor(Color color)
    {
        if (skillIcon != null)
            skillIcon.color = color;
    }

    void UpdateBorderColor(Color color)
    {
        if (skillBorder != null)
            skillBorder.color = color;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (CanBeUnlocked())
            UnlockSkill();

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillTooltip.ShowTooltip(true, rect, this);

        if (!isUnlocked && !isDisabled)
            UpdateBorderColor(Color.white);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillTooltip.ShowTooltip(false, null);

        if (!isUnlocked && !isDisabled)
            UpdateBorderColor(skillBorderColor);
    }

    void OnValidate()
    {
        if (skillData == null) return;

        skillName = skillData.displayName;
        skillIcon.sprite = skillData.icon;
        gameObject.name = "UI_Node - " + skillData.displayName;
    }

    private void OnDisable()
    {
        skillBorder.color = skillBorderColor;
    }
}
