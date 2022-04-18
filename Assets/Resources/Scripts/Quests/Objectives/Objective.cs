using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public delegate void ActivateObjectiveHandler();
public delegate void CompleteObjectiveHandler();


public abstract class Objective
{
    //public Action<ObjectiveInfo> CatalogueObjectiveCompletion;
    //public Action<ObjectiveInfo> CatalogueObjectiveActivation;
    public Action<ObjectiveInfo> RecordObjective;
    public ActivateObjectiveHandler ActivateObjectiveEvent;
    public CompleteObjectiveHandler CompleteObjectiveEvent;
    public ObjectiveInfo Info;
    public string ObjectiveName;
    public bool IsActive
    {
        get 
        {
            if (Info == null)
                Info = new ObjectiveInfo(ObjectiveName);
            return (Info.State & ObjectiveState.Activated) == ObjectiveState.Activated; 
        }
    }

    [SerializeField]
    public virtual bool IsComplete
    {
        get { return true; }
    }

    public ObjectiveState State
    {
        get { return Info.State; }
    }
    

    public virtual void ActivateObjective() 
    {
        if (Info == null) 
            Info = new ObjectiveInfo(ObjectiveName);
        Info.SetActive(true);
        Info.ObjectiveName = ObjectiveName;

        RecordObjective?.Invoke(Info);
    }

    protected virtual void CompleteObjective() 
    {
        Debug.Log("Complete!");
        CompleteObjectiveEvent?.Invoke();
        CompleteObjectiveEvent = null;
        Info.SetComplete();
    }

    public void SetActive(bool active)
    {
        if (Info == null)
            Info= new ObjectiveInfo(ObjectiveName);
        Info.SetActive(active);
    }

}

public enum ObjectiveState
{
    None = 0b_0000_0000,
    Discovered = 0b_0000_0001,
    Activated = 0b_0000_0010,
    Completed = 0b_0000_0100,
    Disqulified = 0b_0000_1000
}

public class ObjectiveInfo
{
    public string ObjectiveName;
    public ObjectiveState State;

    public ObjectiveInfo(string name)
    {
        ObjectiveName = name;
        State = ObjectiveState.None;
    }
    public void SetActive(bool active)
    {
        if (active)
            State |= ObjectiveState.Activated;
        else
        {
            if (State.HasFlag(ObjectiveState.Activated))
                State ^= ObjectiveState.Activated;
        }
    }

    public void SetComplete()
    {
        State |= ObjectiveState.Completed;
        SetActive(false);
    }

    public void SetDiscovered()
    {
        State |= ObjectiveState.Discovered;
    }

    public void SetDisqualified()
    {
        State |= ObjectiveState.Disqulified;
        SetActive(false);
    }
}