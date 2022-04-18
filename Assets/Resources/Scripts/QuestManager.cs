using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public Dictionary<string, Quest> DiscoveredQuests;
    public static QuestManager questManager;
    public static QuestManager Instance
    {
        get
        {
            if (questManager == null)
            {
                questManager = FindObjectOfType<QuestManager>();
                if (questManager == null)
                {
                    //questManager = Instantiate()
                }
            }
            return questManager;
        }
    }

    private void Awake()
    {
        DiscoveredQuests = new Dictionary<string, Quest>();
    }
    public void AddQuest(Quest quest)
    {
        Debug.Log(quest.QuestName + " Started!");
        DiscoveredQuests.Add(quest.QuestName.ToLower(), quest);
    }

    public QuestState CheckQuest(string quest)
    {
        string q = quest.ToLower();
        if (DiscoveredQuests.ContainsKey(q))
            return DiscoveredQuests[q].State;

        return QuestState.None;
    }

    public ObjectiveInfo CheckQuestObjective(string questName, string objectiveName)
    {
        string q = questName.ToLower();
        if (DiscoveredQuests.ContainsKey(q))
        {
            return DiscoveredQuests[q]
                .ObjectiveCatalogue
                .GetInfo(objectiveName);
        }
        return null;
    }
}
