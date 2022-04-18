using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest_1 : Quest
{
    public DialogueObjective obj1, obj3;
    public PushButtonObjective obj2_1, obj2_2;
    public ObjectiveSet obj2;
    protected override void ConstructRootObjectiveSet()
    {
        _rootObjectiveSet = new ObjectiveSet(Name: "TestQuest", OrderRequired: true, onActivate: ObjectiveCatalogue.AddInfo);
    }

    protected override void PopulateObjectives()
    {
        obj2 = new ObjectiveSet(Name: "Push the Buttons", OrderRequired: false);
        obj2.AddObjective(obj2_1);
        obj2.AddObjective(obj2_2);

        _rootObjectiveSet.AddObjective(obj1);
        _rootObjectiveSet.AddObjective(obj2);
        _rootObjectiveSet.AddObjective(obj3);
    }

    protected override void ActivateRootObjective()
    {
        base.ActivateRootObjective();
    }


}
