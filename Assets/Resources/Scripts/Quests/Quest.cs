using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ObjectiveDisqualifiedHandler(ObjectiveInfo i);
public abstract class Quest : MonoBehaviour
{
    //The first Objective in the quest
    protected ObjectiveSet _rootObjectiveSet;
    protected QuestState state;
    public QuestObjectiveCatalogue ObjectiveCatalogue;

    public ObjectiveSet RootObjectiveSet { get { return _rootObjectiveSet; } }
    public QuestState State { get { return state; } }
    public string QuestName { get { return _rootObjectiveSet.ObjectiveName; } }

    private void Awake()
    {
        ObjectiveCatalogue = new QuestObjectiveCatalogue();
    }

    private void Start()
    {
        //TODO: Instantiate quests when they become necessary. Don't just start them at the start of the game.
        ConstructRootObjectiveSet();
        PopulateObjectives();
        ActivateRootObjective();   
    }

    protected virtual void ConstructRootObjectiveSet() {
        Debug.Log("This method shouldn't be called!");
    }

    protected virtual void PopulateObjectives()
    {
        Debug.Log("PopulateObjectives should be called from child class.");
    }

    protected virtual void ActivateRootObjective()
    {
        _rootObjectiveSet.ActivateObjective();
        QuestManager.Instance.AddQuest(this);
        state |= QuestState.Started;
    }

    protected virtual void CompleteQuest()
    {
        state |= QuestState.Completed;
    }
}

public enum QuestState
{
    None = 0b_0000_0000,
    Discovered = 0b_0000_0001,
    Started = 0b_0000_0010,
    Completed = 0b_0000_0100
}

//Keep track of objectives that have been discovered
public class QuestObjectiveCatalogue
{
    public Dictionary<string, ObjectiveInfo> DiscoveredObjectives;

    public QuestObjectiveCatalogue()
    {
        DiscoveredObjectives = new Dictionary<string, ObjectiveInfo>();
    }

    public void AddInfo (ObjectiveInfo oi)
    {
        Debug.Log("Objective " + oi.ObjectiveName + " Added");
        DiscoveredObjectives.Add
            (oi.ObjectiveName
            .Replace(" ", string.Empty)
            .ToLower(), 
            oi);
    }

    public ObjectiveInfo GetInfo (string objectiveName)
    {
        string s = objectiveName.ToLower();
        if (DiscoveredObjectives.ContainsKey(s))
            return DiscoveredObjectives[s];
        return null;
    }
}
