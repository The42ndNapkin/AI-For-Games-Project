using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DecisionNode
{
    public abstract bool Execute();
    public virtual bool isVent() { return false; }
}
