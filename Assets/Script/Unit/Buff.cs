using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class SOBuff : ScriptableObject
{

}
public enum BuffPositiveType
{
    None, Positive, Negative
}
[Serializable]
public abstract class Buff
{
    public virtual BuffPositiveType positiveType
    {
        get { return BuffPositiveType.None; }
    }
    public virtual float UpdateCycle
    {
        get { return 0.25f; }
    }
    public virtual int MaxStack
    {
        get { return 1; }
    }
    public abstract string ID { get; }
    public abstract string IconID { get; }
    int stack = 1;
    public int Stack { get { return stack; } set { stack = value; stack = Mathf.Min(MaxStack, stack); stack = Mathf.Max(1, stack); } }
    public virtual bool Endless
    {
        get { return false; }
    }
    public virtual float Duration
    {
        get { return 1; }
    }
    public Unit Owner;
    public Unit Target;
    public void Init(Unit owner)
    {
        Owner = owner;
    }
    public virtual bool IsGetBuff(Unit unit) { return true; }
    public abstract void BuffStart(Unit unit);
    public abstract void BuffUpdate(Unit unit);
    public abstract void BuffEnd(Unit unit);
    public virtual void BuffOverlay(Unit unit)
    {
        Stack++;
    }
}
