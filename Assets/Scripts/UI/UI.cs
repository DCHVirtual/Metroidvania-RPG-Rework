using UnityEngine;

public class UI : MonoBehaviour
{
    public UI_SkillTree skillTree;
    public UI_SkillTooltip skillTooltip;
    public UI_ItemToolTip itemTooltip;
    PlayerInputSet UI_Input;
    bool skillTreeEnabled = true;
    Player player;

    private void Awake()
    { 
        player = FindAnyObjectByType<Player>();
        skillTree = GetComponentInChildren<UI_SkillTree>(true);
        skillTooltip = GetComponentInChildren<UI_SkillTooltip>();
        itemTooltip = GetComponentInChildren<UI_ItemToolTip>();
        UI_Input = new PlayerInputSet();
        player.input.Player.Disable();
        Time.timeScale = 0;
        ToggleSkillTreeUI();
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
            ToggleSkillTreeUI();
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
}
