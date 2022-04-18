using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveQuestList : MonoBehaviour
{
    public List<Quest> QuestList { get; private set; }


    public void AddQuest (Quest quest)
    {
        QuestList.Add (quest);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
