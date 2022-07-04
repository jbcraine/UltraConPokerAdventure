using System;
using System.Collections;
using System.Collections.Generic;

public class AB
{
    private const string TRUE= "TRUE";
    private const string FALSE = "FALSE";
    private const string QUESTSTATE = "QUESTSTATE";
    private string CONDITION_A;
    private string CONDITION_B;
    private string CONDITION_C;
}
public static class AddressBook
{
    //FIRST, check values
    //If defaults, check methods
    public static bool FindAddress(string key, string[] args = null)
    {
        switch (key)
        {
            case "TRUE" :
                return true;
            case "NOT" :
                return !(FindAddress(args[0]));
            case "QUESTSTATE":
                //Args: QuestName, QuestState
                return FindQuestState(args[0], args[1]);
            case "OBJECTIVESTATE":
                //Args: QuestName, ObjectiveName, ObjectiveState
                return FindObjectiveState(args[0], args[1], args[2]);

            case "CONDITION_A" :
                return ConditionBank.Bank.ConditionA;
            case "CONDITION_B" :
                return ConditionBank.Bank.ConditionB;
            case "CONDITION_C" :
                return ConditionBank.Bank.ConditionC;
            case "GRTR" :
                return (int)FindValue(args[0]) > (int)FindValue(args[1]); 
            default :
                return false;
        }
    }

    public static object FindValue (string key)
    {
        switch (key)
        {
            case "MONEY" :
                return 5000;
            case "NAME" :
                return "Penny";
            default :
                return key;
        }
    }

    public static bool FindObjectiveState(string questName, string objectiveName, string state)
    {
        ObjectiveInfo info = QuestManager.Instance.CheckQuestObjective(questName, objectiveName);
        if (info == null)
            return false;
        switch (state)
        {
            case "DISCOVERED":
                return info.State.HasFlag(ObjectiveState.Discovered);
            case "ACTIVATED":
                return info.State.HasFlag(ObjectiveState.Activated);
            case "COMPLETED":
                return info.State.HasFlag(ObjectiveState.Completed);
            case "DISQUALIFIED":
                return info.State.HasFlag(ObjectiveState.Disqulified);
            default: 
                return false;
        }
    }

    public static bool FindQuestState(string questName, string state)
    {
        QuestState q = QuestManager.Instance.CheckQuest(questName);

        switch (state)
        {
            case "DISCOVERED":
                return q.HasFlag(QuestState.Discovered);
            case "STARTED":
                return q.HasFlag(QuestState.Started);
            case "COMPLETED":
                return q.HasFlag(QuestState.Completed);
            default:
                return false;
        }
    }
    
}
