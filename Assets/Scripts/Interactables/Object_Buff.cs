using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class Buff
{
    public StatType type;
    public float value;
}

public class Object_Buff : MonoBehaviour
{
    SpriteRenderer sr;
    Entity_Stats stats;
    Guid uniqueID;

    [Header("Buff details")]
    [SerializeField] Buff[] buffs;
    [SerializeField] string buffName;
    [SerializeField] float buffDuration = 4f;
    [SerializeField] bool canBeUsed = true;


    [Header("Floaty Movement")]
    [SerializeField] float floatSpeed = 1f;
    [SerializeField] float floatRange = .1f;
    Vector3 startPos;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        startPos = transform.position;
        uniqueID = Guid.NewGuid();
    }

    // Update is called once per frame
    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatRange;
        transform.position = startPos + new Vector3(0, yOffset);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canBeUsed) return;
        stats = collision.GetComponent<Entity_Stats>();
        StartCoroutine(BuffCo(buffDuration));
    }

    IEnumerator BuffCo(float duration)
    {
        canBeUsed = false;
        sr.color = Color.clear;

        //Apply buffs
        foreach (var buff in buffs)
            stats.GetStatByType(buff.type).AddModifier(buff.value, buffName, uniqueID);

        yield return new WaitForSeconds(duration);

        //Remove Buffs
        foreach (var buff in buffs)
            stats.GetStatByType(buff.type).RemoveModifier(uniqueID);

        Destroy(gameObject);
    }
}
