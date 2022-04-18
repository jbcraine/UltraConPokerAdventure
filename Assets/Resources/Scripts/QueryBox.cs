using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public delegate void QuerySubmit(string query);
public class QueryBox : MonoBehaviour
{
    public string queryString;
    public QuerySubmit querySubmitted;
    public TMP_InputField input;

    // Start is called before the first frame update
    void Start()
    {
        querySubmitted += DialogueManager.Instance.QuerySubmitted;
        querySubmitted += GetComponentInParent<ResponseBox>().DestroyResponses;
        transform.localScale = Vector2.zero;
    }

    public void UpdateText()
    {
        queryString = input.text;
    }

    public void SubmitQuery()
    {
        querySubmitted(queryString);
        SetStateToDefault();
    }

    public void SetStateToDefault()
    {
        queryString = "";
        input.text = "";
    }
}
