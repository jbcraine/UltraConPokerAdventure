using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class ResponseOption : MonoBehaviour
{
    public OnResponseSelect ResponseSelected;
    public string responseTag;
    public Sprite background;
    public void Init(string text, string tag)
    {
        TMP_Text tmptext = GetComponentInChildren<TMP_Text>();
        tmptext.text = text;
        responseTag = tag;
        ResponseSelected += DialogueManager.Instance.GoToTagAndRead;
    }

    public void Select()
    {
        ResponseSelected(responseTag);
        Debug.Log("Click");
    }

    public void OnMouseDown()
    {
        Select();
    }

    public void OnMouseOver()
    {
      
    }

    public void OnMouseExit()
    {
    
    }
}
