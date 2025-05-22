using UnityEngine;

public class UI_MiniHealthBar : MonoBehaviour
{
    Entity entity;

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
    }

    private void OnEnable()
    {
        entity.OnFlip += FlipHealthBar;
    }

    private void OnDisable()
    {
        entity.OnFlip += FlipHealthBar;
    }

    void FlipHealthBar()
    {
        var scl = transform.localScale;
        transform.localScale = new Vector3(scl.x * -1, scl.y, scl.z);
    }
}
