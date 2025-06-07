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
        if (!show && rect != null)
        {
            rect.position = new Vector2(9999, 9999);
            return;
        }

        UpdatePosition(hoverRect);
    }

    void UpdatePosition(RectTransform hoverRect)
    {
        if (hoverRect == null) return;
        var canvas = GetComponentInParent<Canvas>();

        float screenCenterX = Screen.width / 2f;
        float screenTop = Screen.height;
        float screenBottom = 0;

        var targetPosition = RectTransformUtility.WorldToScreenPoint(null, hoverRect.position);

        bool placeLeft = targetPosition.x > screenCenterX;
        rect.pivot = new Vector2(placeLeft ? 1f : 0f, 0.5f);
        targetPosition.x +=  (placeLeft ? -offset.x : offset.x) * canvas.scaleFactor;
        
        float tooltipHalfHeight = rect.sizeDelta.y / 2f * canvas.scaleFactor;
        float tooltipTop = targetPosition.y + tooltipHalfHeight;
        float tooltipBottom = targetPosition.y - tooltipHalfHeight;

        if (tooltipTop > screenTop)
            targetPosition.y = screenTop - tooltipHalfHeight - offset.y;
        if (tooltipBottom < screenBottom)
            targetPosition.y = screenBottom + tooltipHalfHeight + offset.y;

        RectTransformUtility.ScreenPointToLocalPointInRectangle
            (
                canvas.transform as RectTransform,
                targetPosition,
                null,
                out var localPosition
            );


        rect.anchoredPosition = localPosition;
    }

    protected string GetColoredText(string text, string hexColor)
    {
        return $"<color={hexColor}>{text}</color>";
    }
}
