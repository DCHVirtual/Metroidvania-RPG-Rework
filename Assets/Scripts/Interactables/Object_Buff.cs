using UnityEngine;

public class Object_Buff : MonoBehaviour
{
    Player_Stats stats;
    Data_BuffEffect buffEffect;

    [Header("Buff details")]
    [SerializeField] Buff[] buffs;
    [SerializeField] string source;
    [SerializeField] float duration;


    [Header("Floaty Movement")]
    [SerializeField] float floatSpeed = 1f;
    [SerializeField] float floatRange = .1f;
    Vector3 startPos;

    private void Awake()
    {
        startPos = transform.position;
        buffEffect = new Data_BuffEffect(buffs, duration, source);
    }

    // Update is called once per frame
    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatRange;
        transform.position = startPos + new Vector3(0, yOffset);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<SkillObject_TimeEcho>() == true) return;

        stats = collision.GetComponent<Player_Stats>();
        stats.ApplyBuff(buffEffect);
        Destroy(gameObject);
    }
}
