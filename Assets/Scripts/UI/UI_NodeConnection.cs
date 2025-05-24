using UnityEngine;
using UnityEngine.UI;

public enum NodeDirection
{
    None,
    UpLeft,
    Up,
    UpRight,
    Left,
    Right,
    DownLeft,
    Down,
    DownRight
}

public class UI_NodeConnection : MonoBehaviour
{
    [SerializeField] RectTransform rotationPoint;
    [SerializeField] RectTransform connectLength;
    [SerializeField] RectTransform childNodePoint;

    public void DirectConnection(NodeDirection dir, float length, float rotationOffset)
    {
        bool shouldBeActive = dir != NodeDirection.None;
        float finalLength = shouldBeActive ? length : 0;
        float angle = GetDirectionAngle(dir);

        rotationPoint.localRotation = Quaternion.Euler(0, 0, angle + rotationOffset);
        connectLength.sizeDelta = new Vector2(finalLength, connectLength.sizeDelta.y);
    }

    public Image GetConnectionImage() => connectLength.GetComponent<Image>();

    public Vector2 GetConnectionPoint(RectTransform rect)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle
            (
                rect.parent as RectTransform,
                childNodePoint.position,
                null,
                out var localPosition
            );
        return localPosition;
    }

    float GetDirectionAngle(NodeDirection dir)
    {
        switch (dir)
        {
            case NodeDirection.UpLeft: return 135f;
            case NodeDirection.Up: return 90f;
            case NodeDirection.UpRight: return 45f;
            case NodeDirection.Left: return 180f;
            case NodeDirection.Right: return 0f;
            case NodeDirection.DownLeft: return -135f;
            case NodeDirection.Down: return -90f;
            case NodeDirection.DownRight: return -45f;
            default: return 0f;
        }
    }
}
