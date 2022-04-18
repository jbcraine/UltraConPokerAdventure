using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Quest[] quests = FindObjectsOfType<Quest>();
        foreach (Quest quest in quests)
            QuestManager.Instance.AddQuest(quest);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
