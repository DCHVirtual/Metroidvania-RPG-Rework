using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Reset Skills", fileName = "Item Effect Data - Reset Skills")]
public class Data_ItemEffect_ResetSkills : Data_ItemEffectSO
{
    public override void ExecuteEffect()
    {
        FindFirstObjectByType<UI>().RefundAllSkills();
    }
}
