using UnityEngine;

public class Data_ItemEffectSO : ScriptableObject
{
    [TextArea]
    [SerializeField] protected string description;
    protected Player player;

    public virtual string GetDescription()
    {
        return description;
    }

    public virtual bool CanBeUsed()
    {
        return true;
    }

    public virtual void ExecuteEffect()
    {

    }

    public virtual void Subscribe(Player player)
    {
        this.player = player;
    }

    public virtual void Unsubscribe()
    {

    }
}
