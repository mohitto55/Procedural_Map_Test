using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Fast : Buff
{
    public override void BuffEnd(Unit unit)
    {
        unit.SpeedBuffs -= Speed;
    }

    public override void BuffStart(Unit unit)
    {
        
        unit.SpeedBuffs += Speed;
    }
    int Speed (Unit unit)
    {
        return 5 + Stack;
    }
    public override void BuffUpdate(Unit unit)
    {
    }

    public override string ID
    {
        get { return "Fast"; }
    }
    public override string IconID
    {
        get { return null; }
    }
}
