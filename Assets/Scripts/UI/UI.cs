using System.Text;
using UnityEngine;

public class UI : MonoBehaviour
{
    [HideInInspector] public UI_SkillTree skillTree;
    [HideInInspector] public UI_Inventory uiInventory;
    [HideInInspector] public UI_Storage uiStorage;
    [HideInInspector] public UI_Craft uiCraft;
    [HideInInspector] public UI_Merchant uiMerchant;
    [HideInInspector] public UI_SkillTooltip skillTooltip;
    [HideInInspector] public UI_ItemToolTip itemToolTip;
    [HideInInspector] public UI_StatToolTip statTooltip;
    [HideInInspector] public PlayerInputSet UI_Input;
    bool skillTreeEnabled = true;
    bool inventoryEnabled = true;
    [HideInInspector] public Player player;

    private void Awake()
    {
        player = FindAnyObjectByType<Player>();
        skillTree = GetComponentInChildren<UI_SkillTree>(true);
        uiInventory = GetComponentInChildren<UI_Inventory>(true);
        uiStorage = GetComponentInChildren<UI_Storage>(true);
        uiCraft = GetComponentInChildren<UI_Craft>(true);
        uiMerchant = GetComponentInChildren<UI_Merchant>(true);
        skillTooltip = GetComponentInChildren<UI_SkillTooltip>();
        itemToolTip = GetComponentInChildren<UI_ItemToolTip>();
        statTooltip = GetComponentInChildren<UI_StatToolTip>();
        UI_Input = new PlayerInputSet();
    }

    private void Start()
    {
        ToggleSkillTreeUI();
        ToggleInventoryUI();
    }

    public void SwitchOffAllTooltips()
    {
        skillTooltip.ShowTooltip(false, null);
        itemToolTip.ShowTooltip(false, null);
        statTooltip.ShowTooltip(false, null);
    }

    private void OnEnable()     
    {
        UI_Input.UI.Enable();
    }

    private void OnDisable()
    {
        UI_Input.UI.Disable();
    }

    private void Update()
    {
        if (UI_Input.UI.SkillTree.WasPressedThisFrame())
        {
            if (inventoryEnabled)
                ToggleInventoryUI();
            ToggleSkillTreeUI();
        }
        if (UI_Input.UI.Inventory.WasPressedThisFrame())
        {
            uiInventory.UpdateUI();
            if (skillTreeEnabled)
                ToggleSkillTreeUI();
            ToggleInventoryUI();
        }
    }

    public void ToggleSkillTreeUI()
    {
        skillTreeEnabled = !skillTreeEnabled;
        skillTree.gameObject.SetActive(skillTreeEnabled);
        skillTooltip.ShowTooltip(false, null);
        Time.timeScale = 1 - Time.timeScale;

        if (player.input.Player.enabled)
            player.input.Player.Disable();
        else
            player.input.Player.Enable();
    }
    public void ToggleInventoryUI()
    {
        inventoryEnabled = !inventoryEnabled;
        uiInventory.gameObject.SetActive(inventoryEnabled);
        itemToolTip.ShowTooltip(false, null);
        statTooltip.ShowTooltip(false, null);
        Time.timeScale = 1 - Time.timeScale;

        if (player.input.Player.enabled)
            player.input.Player.Disable();
        else
            player.input.Player.Enable();
    }

    [ContextMenu("Refund All Skills")]
    public void RefundAllSkills()
    {
        var skillNodes = GetComponentsInChildren<UI_TreeNode>(true);

        foreach (var node in skillNodes)
        {
            node.RefundSkill();
        }

        skillTree.ApplyRedToDisabledConnex();
    }

    #region Info functions
    public string GetStatNameByType(StatType type)
    {
        switch (type)
        {
            case StatType.Armor:
            case StatType.Evasion:
            case StatType.Agility:
            case StatType.Intelligence:
            case StatType.Vitality:
            case StatType.Damage:
            case StatType.Strength: return type.ToString();
            case StatType.MaxHealth: return "Max Health";
            case StatType.HealthRegen: return "Health Regeneration";
            case StatType.AttackSpeed: return "Attack Speed";
            case StatType.CritChance: return "Critical Chance";
            case StatType.CritDamage: return "Critical Damage";
            case StatType.ArmorReduction: return "Armor Reduction";
            case StatType.FireDamage: return "Fire Damage";
            case StatType.IceDamage: return "Ice Damage";
            case StatType.LightningDamage: return "Lightning Damage";
            case StatType.ElementalDamage: return "Elemental Damage";
            case StatType.IceResistance: return "Ice Resistance";
            case StatType.FireResistance: return "Fire Resistance";
            case StatType.LightningResistance: return "Lightning Resistance";
            default: return "";
        }
    }

    public string PercentString(StatType type)
    {
        switch (type)
        {
            case StatType.CritChance:
            case StatType.CritDamage:
            case StatType.ArmorReduction:
            case StatType.IceResistance:
            case StatType.FireResistance:
            case StatType.LightningResistance:
            case StatType.AttackSpeed:
            case StatType.Evasion:
                return "%";
            default:
                return "";
        }
    }

    public string GetItemInfo(Inventory_Item item)
    {
        if (item.itemData.type == ItemType.Material)
            return "Used for crafting.";

        if (item.itemData.type == ItemType.Consumable)
            return item.itemData.effect.GetDescription();

        StringBuilder sb = new StringBuilder();

        foreach (var mod in item.modifiers)
            sb.AppendLine($"+{mod.value * GetModMultiplier(mod.statType)}{PercentString(mod.statType)} {GetStatNameByType(mod.statType)}");

        if (item.itemEffect != null)
        {
            sb.AppendLine("\nUnique Effect:");
            sb.AppendLine(item.itemEffect.GetDescription());
        }

        return sb.ToString();
    }

    float GetModMultiplier(StatType type)
    {
        switch (type)
        {
            case StatType.AttackSpeed:
            case StatType.ArmorReduction:
            case StatType.CritChance:
                return 100;
            default: return 1;
        }
    }
    #endregion
}
