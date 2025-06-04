using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    Player player;
    UI_SkillSlot[] skillSlots;
    
    [SerializeField] RectTransform healthRect;
    [SerializeField] Slider healthSlider;
    [SerializeField] TextMeshProUGUI healthText;
    private void Awake()
    {
        skillSlots = GetComponentsInChildren<UI_SkillSlot>();
    }

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        player.health.OnHealthBarUpdate += UpdateHealthBar;
        
    }

    void UpdateHealthBar()
    {
        float currHealth = player.health.GetCurrentHealth();
        float maxHealth = player.stats.GetMaxHealth();
        healthSlider.value = player.health.GetHealthPercent();
        healthText.text = $"{Mathf.RoundToInt(currHealth)}/{Mathf.RoundToInt(maxHealth)}";

        
    }

    public void AssignSkillSlot(SkillData_SO skill)
    {
        if (skill.upgradeData.upgradeType == SkillUpgradeType.Dash)
            skillSlots[0].SetupSkillSlot(skill);
        else if (skill.upgradeData.upgradeType == SkillUpgradeType.TimeEcho ||
                 skill.upgradeData.upgradeType == SkillUpgradeType.Shard)
            skillSlots[1].SetupSkillSlot(skill);
        else if (skill.upgradeData.upgradeType == SkillUpgradeType.SwordThrow)
            skillSlots[2].SetupSkillSlot(skill);
        else if (skill.upgradeData.upgradeType == SkillUpgradeType.Domain_Slowdown)
            skillSlots[3].SetupSkillSlot(skill);

    }

    public UI_SkillSlot GetSkillSlot(SkillType skillType)
    {
        return skillSlots.Where(slot => slot.skillType == skillType).FirstOrDefault();
    }
}
