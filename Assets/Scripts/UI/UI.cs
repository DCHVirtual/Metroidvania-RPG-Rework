using UnityEngine;

public class UI : MonoBehaviour
{
    public UI_SkillTree skillTree;
    public UI_SkillTooltip skillTooltip;
    bool skillTreeEnabled = true;

    private void Awake()
    {
        skillTree = GetComponentInChildren<UI_SkillTree>(true);
        skillTooltip = GetComponentInChildren<UI_SkillTooltip>();
    }

    public void ToggleSkillTreeUI()
    {
        skillTreeEnabled = !skillTreeEnabled;
        skillTree.gameObject.SetActive(skillTreeEnabled);
        skillTooltip.ShowTooltip(false, null);
    }
}
