using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void InteractEventHandler(int count);

[System.Serializable]
public class InteractCounter : MonoBehaviour, IPointerDownHandler
{
    public InteractEventHandler Interacted;
    public int counter = 0;

    private void Start()
    {

    }
    public int Counter
        { get { return counter; } }

    public void Interact()
    {
        counter++;
        Interacted?.Invoke(Counter);
    }

    public void OnPointerDown (PointerEventData ped)
    {
        counter++;
        Interacted?.Invoke(counter);
    }
}
