using Unity.Cinemachine;
using UnityEngine;

public class UI_ToolTip : MonoBehaviour
{
    RectTransform rect;
    protected UI ui;
    [SerializeField] Vector2 offset = new Vector2(250, 50);

    protected virtual void Awake()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
        ShowTooltip(false, rect);
    }

    public virtual void ShowTooltip(bool show, RectTransform hoverRect)
    {
        if (!show)
        {
            rect.position = new Vector2(9999, 9999);
            return;
        }

        UpdatePosition(hoverRect);
    }

    void UpdatePosition(RectTransform hoverRect)
    {
        float screenCenterX = Screen.width / 2f;
        float screenTop = Screen.height;
        float screenBottom = 0;
        

        var targetPosition = hoverRect.position;
        targetPosition.x += targetPosition.x > screenCenterX ? -offset.x : offset.x;
        
        float tooltipHalfHeight = rect.sizeDelta.y / 2f;
        float tooltipTop = targetPosition.y + tooltipHalfHeight;
        float tooltipBottom = targetPosition.y - tooltipHalfHeight;

        if (tooltipTop > screenTop)
            targetPosition.y = screenTop - tooltipHalfHeight - offset.y;
        if (tooltipBottom < screenBottom)
            targetPosition.y = screenBottom + tooltipHalfHeight + offset.y;


        rect.position = targetPosition;
    }

    protected string GetColoredText(string text, string hexColor)
    {
        return $"<color={hexColor}>{text}</color>";
    }
}
