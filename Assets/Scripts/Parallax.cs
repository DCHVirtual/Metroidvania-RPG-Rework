using UnityEngine;

public class Parallax : MonoBehaviour
{
    Camera cam;
    [SerializeField] float parallaxEffect;
    float xPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        xPosition = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToMove = cam.transform.position.x * parallaxEffect;
        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);
    }
}
