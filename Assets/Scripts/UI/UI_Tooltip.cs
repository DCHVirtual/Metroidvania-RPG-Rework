using Unity.Cinemachine;
using UnityEngine;

public class UI_Tooltip : MonoBehaviour
{
    RectTransform rect;
    [SerializeField] Vector2 offset = new Vector2(300, 20);

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        ShowTooltip(false, rect);
    }

    public void ShowTooltip(bool show, RectTransform targetRect)
    {
        if (!show)
        {
            rect.position = new Vector2(9999, 9999);
            return;
        }

        UpdatePosition(targetRect);
    }

    void UpdatePosition(RectTransform targetRect)
    {
        float screenCenterX = Screen.width / 2f;
        float screenTop = Screen.height;
        float screenBottom = 0;
        

        var targetPosition = targetRect.position;
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
}
