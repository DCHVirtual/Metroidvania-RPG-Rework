using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillSlot : MonoBehaviour
{
    UI ui;
    Image skillIcon;
    RectTransform rect;
    SkillData_SO skillData;
    Player_SkillManager skillManager;
    Skill skill;
    public SkillType skillType;
    public bool active = false;

    [SerializeField] Image cooldownImage;
    [SerializeField] TextMeshProUGUI inputKeyText;
    [SerializeField] string inputKeyName;

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        skillIcon = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        skillManager = FindFirstObjectByType<Player_SkillManager>();
        cooldownImage.fillAmount = 1;
        //skill = skillManager.GetSkillByType(skillType);
    }

    private void Update()
    {
        if (skill == null) return;

        float cooldown = skill.cooldown;
        float elapsedTime = Time.time - skill.lastTimeUsed;

        cooldownImage.fillAmount = (cooldown - elapsedTime) / cooldown;
    }

    public void SetupSkillSlot(SkillData_SO selectedSkill)
    {
        if (selectedSkill == null) return;

        active = true;
        skillType = selectedSkill.skillType;

        skillData = selectedSkill;
        skill = skillManager.GetSkillByType(skillType);

        inputKeyText.text = inputKeyName;
        skillIcon.sprite = selectedSkill.icon;
    }
}
