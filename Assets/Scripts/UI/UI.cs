using UnityEngine;

public class UI : MonoBehaviour
{
    public UI_Tooltip skillTooltip;

    private void Awake()
    {
        skillTooltip = GetComponentInChildren<UI_Tooltip>();
    }
}
