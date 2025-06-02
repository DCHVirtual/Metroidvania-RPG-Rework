using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class UI_SkillTooltip : UI_ToolTip
{
    UI_SkillTree skillTree;

    [SerializeField] TextMeshProUGUI skillName;
    [SerializeField] TextMeshProUGUI skillDescriptione;
    [SerializeField] TextMeshProUGUI skillRequirements;
    [Space]
    [SerializeField] string conditionsMetHex;
    [SerializeField] string conditionsUnmetHex;
    [SerializeField] string importantInfoHex;
    [SerializeField] Color testColor;
    string disabledSkillText = "This path has been disabled\nSkill cannot be unlocked";

    protected override void Awake()
    {
        base.Awake();
        skillTree = ui.GetComponentInChildren<UI_SkillTree>(true);
    }
    public override void ShowTooltip(bool show, RectTransform hoverRect)
    {
        base.ShowTooltip(show, hoverRect);
    }

    public void ShowTooltip(bool show, RectTransform hoverRect, UI_TreeNode node)
    {
        base.ShowTooltip(show, hoverRect);

        if (show == false) return;

        skillName.text = node.skillData.displayName;
        skillDescriptione.text = node.skillData.description;

        string disabledText = $"<color={conditionsUnmetHex}>{disabledSkillText}</color>";
        string requirements = node.isDisabled ? disabledText : GetRequirements(node.skillData.cost, node.neededNodes, node.conflictNodes);

        skillRequirements.text = requirements;
            
    }

    string GetRequirements(int skillCost, UI_TreeNode[] neededNodes, UI_TreeNode[] conflictNodes)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Requirements:");

        string costColor = skillTree.HaveEnoughSkillPoints(skillCost) ? conditionsMetHex : conditionsUnmetHex;

        sb.AppendLine(GetColoredText($"{skillCost} skill point(s)", costColor));

        foreach (var node in neededNodes)
        {
            string nodeColor = node.isUnlocked ? conditionsMetHex : conditionsUnmetHex;
            sb.AppendLine(GetColoredText($"{node.skillData.displayName}", nodeColor));
        }

        if (conflictNodes.Length == 0)
            return sb.ToString();

        sb.AppendLine();
        sb.AppendLine("Locks out:");

        foreach (var node in conflictNodes)
            sb.AppendLine(GetColoredText($"{node.skillData.displayName}", importantInfoHex));

        return sb.ToString();
    }
}
