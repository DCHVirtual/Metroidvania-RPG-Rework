using UnityEngine;

public class NPC : MonoBehaviour
{
    protected Transform player;
    protected UI ui;
    protected Animator anim;
    protected bool canInteract = false;

    [SerializeField] Transform npc;
    [SerializeField] GameObject interactToolTip;

    [Header("Floaty ToolTip")]
    [SerializeField] float floatSpeed = 8f;
    [SerializeField] float floatRange = .1f;
    Vector3 startPos;

    protected virtual void Awake()
    {
        ui = FindFirstObjectByType<UI>();
        startPos = interactToolTip.transform.position;
        interactToolTip.SetActive(false);
        anim = GetComponentInChildren<Animator>();
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        player = other.transform;
        interactToolTip.SetActive(true);
    }

    protected virtual void Update()
    {
        HandleToolTipFloat();
        FacePlayer();
    }

    void FacePlayer()
    {
        if (player == null)
            return;

        if (transform.localScale.x != Mathf.Sign(player.position.x - transform.position.x))
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            interactToolTip.transform.localScale = new Vector3(-interactToolTip.transform.localScale.x, 1, 1);
        }
    }

    void HandleToolTipFloat()
    {
        if (interactToolTip.activeSelf)
        {
            float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatRange;
            interactToolTip.transform.position = startPos + new Vector3(0, yOffset);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        interactToolTip.SetActive(false);
    }
}
