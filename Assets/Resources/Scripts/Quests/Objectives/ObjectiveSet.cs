using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class ObjectiveSet : Objective
{
    public bool OrderRequired;
    public List<Objective> Objectives;
    public int CompletedObjectives;
    public override bool IsComplete
    {
        get { return FindNextIncompleteObjective() == null; }
    }

    public override void ActivateObjective()
    {
        if (IsComplete)
            return;

        if (!IsActive)
            base.ActivateObjective();


        if (OrderRequired)
        {
            Objective next = FindNextIncompleteObjective();
            next.RecordObjective = this.RecordObjective;
            next.ActivateObjective();
            next.CompleteObjectiveEvent += CompleteObjective;
        }
        else
        {
            foreach (Objective o in Objectives)
            {
                o.RecordObjective = this.RecordObjective;
                o.ActivateObjective();
                o.CompleteObjectiveEvent += CompleteUnorderedObjective;
            }
        }
    }

    private Objective FindNextIncompleteObjective ()
    {
        foreach (Objective o in Objectives)
        {
            if (!o.IsComplete)
                return o;
        }

        return null;
    }

    protected override void CompleteObjective()
    {
        //ERROR: calling base.CompleteObjective from here will cause two calls. One from here, and one from the Objective itself. Careful!
        //base.CompleteObjective();
        ActivateObjective();
    }

    public void CompleteUnorderedObjective()
    {
        if (++CompletedObjectives == Objectives.Count)
        {
            base.CompleteObjective();
            ActivateObjective();
        }
    }

    public ObjectiveSet (string Name, bool OrderRequired) {
        this.ObjectiveName = Name;
        this.OrderRequired = OrderRequired;
        this.SetActive(true);
        Objectives = new List<Objective>();
    }

    public ObjectiveSet (string Name, bool OrderRequired, Action<ObjectiveInfo> onActivate)
    {
        this.ObjectiveName= Name;
        this.OrderRequired= OrderRequired;
        this.SetActive(true);
        Objectives = new List<Objective>();
        RecordObjective = onActivate;
    }


    public void AddObjective (Objective objective)
    {
        Objectives.Add(objective);
    }
}
