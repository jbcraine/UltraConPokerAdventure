using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueObjective : Objective
{
    [SerializeField]
    public string TargetTag;
    public bool TagRead { get; private set; }
    public override bool IsComplete
    {
        get { return TagRead; }
    }

    public override void ActivateObjective()
    {
        base.ActivateObjective();
        DialogueManager.Instance.DialogueEventRaised += CheckTag;
    }

    private void CheckTag (string tag)
    {
        if (tag == TargetTag)
        {
            TagRead = true;
            DialogueManager.Instance.DialogueEventRaised -= CheckTag;
            CompleteObjective();
        }
    }

    protected override void CompleteObjective()
    {
        base.CompleteObjective();
    }

    public DialogueObjective (string target)
    {
        TargetTag = target;
        TagRead = false;
    }
}
