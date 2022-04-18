using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InteractNPC : MonoBehaviour, IInteractable
{
    private DialogueSO NPCdialogue;
    public CharacterProfile characterInfo;
    // Start is called before the first frame update
    void Start()
    {
        NPCdialogue = characterInfo.dialogue;
    }

    public void Interact()
    {
        DialogueManager.Instance.BeginConversation(NPCdialogue);
    }

    public void OnPointerDown (PointerEventData ped)
    {
        Interact();
    }



}
