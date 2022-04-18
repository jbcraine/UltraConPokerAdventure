using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void OnPointerDownHandler();

public class DialogueBoxButton : MonoBehaviour, IPointerClickHandler
{
    public OnPointerDownHandler PointerDown;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick (PointerEventData ped)
    {
        PointerDown?.Invoke();
    }
}
