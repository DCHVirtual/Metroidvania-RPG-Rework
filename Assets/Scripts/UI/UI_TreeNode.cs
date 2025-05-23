using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    UI ui;
    RectTransform rect;

    [SerializeField] SkillData_SO skillData;
    [SerializeField] string skillName;
    [SerializeField] Image skillIcon;
    [SerializeField] Image skillBorder;
    [SerializeField] Color skillLockedColor;
    [SerializeField] Color skillBorderColor;
    public bool isUnlocked;
    public bool isDisabled;

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
    }

    void UnlockSkill()
    {
        isUnlocked = true;
        UpdateIconColor(Color.white);
    }

    bool CanBeUnlocked()
    {
        if (isDisabled || isUnlocked)
            return false;

        return true;
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
        ui.skillTooltip.ShowTooltip(true, rect);

        if (!isUnlocked)
            UpdateBorderColor(Color.white);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillTooltip.ShowTooltip(false, null);

        if (!isUnlocked)
            UpdateBorderColor(skillBorderColor);
    }

    void OnValidate()
    {
        if (skillData == null) return;

        skillName = skillData.displayName;
        skillIcon.sprite = skillData.icon;
        gameObject.name = "UI_TreeNode - " + skillData.displayName;
    }
}
