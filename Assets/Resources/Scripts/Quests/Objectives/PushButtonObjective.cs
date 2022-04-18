using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PushButtonObjective : Objective
{
    public InteractCounter counter;
    public int count = 0;
    public int Goal;

    public override bool IsComplete
    {
        get { return count >= Goal; }
    }

    public override void ActivateObjective()
    {
        base.ActivateObjective();
        counter.Interacted += CheckCount;
    }

    protected override void CompleteObjective()
    {
        base.CompleteObjective();
    }

    public void CheckCount(int count)
    {
        this.count = count;
        if (IsComplete)
            CompleteObjective();
    }
}
