using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Stats : Entity_Stats
{
    private List<string> activeBuffs = new List<string>();
    Inventory_Player inventory;

    protected override void Awake()
    {
        base.Awake();
        inventory = GetComponent<Inventory_Player>();
    }
    public bool CanApplyBuff(string source)
    {
        return activeBuffs.Contains(source) == false;
    }

    public void ApplyBuff(Data_BuffEffect buffEffect)
    {
        StartCoroutine(BuffCo(buffEffect));
    }

    IEnumerator BuffCo(Data_BuffEffect buffEffect)
    {
        activeBuffs.Add(buffEffect.source);
        buffEffect.ApplyBuffs(this);
        yield return new WaitForSeconds(buffEffect.duration);
        buffEffect.RemoveBuffs(this);
        activeBuffs.Remove(buffEffect.source);
        //inventory.UpdateUI();
    }
}