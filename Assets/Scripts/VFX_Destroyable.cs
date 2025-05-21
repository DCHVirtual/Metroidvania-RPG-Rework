using UnityEngine;

public class VFX_Destroyable : MonoBehaviour
{
    [SerializeField] float destroyTime = 1f;
    private void Update()
    {
        Destroy(gameObject, destroyTime);
    }
}
