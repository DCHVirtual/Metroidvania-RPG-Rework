using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UI_NodeConnectDetails
{
    public UI_NodeConnectHandler childNode;
    public UI_NodeConnection connection;
    public NodeDirection direction;
    [Range(100f, 350f)] public float length;
    [Range(-35f, 35f)] public float rotationOffset;
}

public class UI_NodeConnectHandler : MonoBehaviour
{
    RectTransform myRect => GetComponent<RectTransform>();
    public UI_TreeNode treeNode { get; private set; }
    [field: SerializeField] public UI_NodeConnectDetails[] connectDetails {  get; private set; }

    Image connectionImage;
    Color originalConnectionColor = new Color(0.4213836f, 0.4213836f, 0.4213836f, 1);

    private void Awake()
    {
        treeNode = GetComponent<UI_TreeNode>();
    }

    /*private void OnValidate()
    {

        if (connectDetails.Length == 0)
            return;

        UpdateConnections();
    }*/

    public void UpdateConnections()
    {
        for (int i = 0; i < connectDetails.Length; i++)
        {
            var detail = connectDetails[i];
            var connection = detail.connection;

            Vector2 targetPosition = connection.GetConnectionPoint(myRect);
            Image connectionImage = connection.GetConnectionImage();

            connection.DirectConnection(detail.direction, detail.length, detail.rotationOffset);

            if (detail.childNode == null) continue;

            detail.childNode.SetPosition(targetPosition);
            detail.childNode.SetConnectionImage(connectionImage);
            detail.childNode.transform.SetAsLastSibling();
        }
    }

    public void UpdateAllConnections()
    {
        UpdateConnections();

        foreach (var detail in connectDetails)
        {
            if (detail.childNode == null)
                continue;
            detail.childNode.UpdateAllConnections();
        }
    }

    public void UnlockConnectionImage(bool unlocked)
    {
        if (connectionImage == null) return;

        connectionImage.color = unlocked ? Color.white : originalConnectionColor;
    }

    public void SetDisabledConnection()
    {
        if (treeNode.isDisabled && connectionImage != null)
            connectionImage.color = Color.red;
        else if (connectionImage != null)
            connectionImage.color = originalConnectionColor;

        foreach (var detail in connectDetails)
        {
            if (detail.childNode != null)
                detail.childNode.SetDisabledConnection();
        }
    }

    public void SetConnectionImage(Image image) => connectionImage = image;

    public void SetPosition(Vector2 position) => myRect.anchoredPosition = position;

    internal void SetStartConnexColor()
    {
        if (connectionImage != null)
            connectionImage.color = originalConnectionColor;

        foreach (var detail in connectDetails)
        {
            if (detail.childNode != null)
                detail.childNode.SetStartConnexColor();
        }
    }
}
