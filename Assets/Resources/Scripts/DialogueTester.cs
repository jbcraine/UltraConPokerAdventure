using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DialogueTester : MonoBehaviour
{
    public DialogueSO dialogueFile;

    // Start is called before the first frame update
    void Start()
    {
        DialogueManager.Instance.BeginConversation(dialogueFile);
        
        /*
        while ((s = DialogueManager.Instance.ReadDialogue()) != null)
        {
            Debug.Log(s); 
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
