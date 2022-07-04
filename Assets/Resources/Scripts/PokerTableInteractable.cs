using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PokerTableInteractable : MonoBehaviour, IInteractable
{
    [SerializeField]
    PokerGameEnum gametype;
    [SerializeField]
    PokerStarter starter;
    private bool _enabled = true;
    public void OnPointerDown(PointerEventData ped)
    {
        Interact();
    }

    public void Interact()
    {
        if (!_enabled)
            return;

        //Launch poker game
        starter.BeginInstance();
        //Disable movement system
        _enabled = false;
    }
}
