using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResponseBox : MonoBehaviour
{
    public GameObject responsePrefab;
    public VerticalLayoutGroup responseContentList;
    public List<GameObject> responseList = new List<GameObject>();
    public QueryBox query;
    public int offset = 0;

    // Start is called before the first frame update
    void Start()
    {
        DialogueManager.Instance.GivePlayerChoices = GetChoices;
        responseContentList = GetComponentInChildren<VerticalLayoutGroup>();
        query = GetComponentInChildren<QueryBox>();
        DialogueManager.Instance.QueryDetected += ActivateQueryBox;
    }

    public void GenerateResponses (string[] responseStrings)
    {
        foreach (string response in responseStrings)
        {
            GameObject responseOption = Instantiate(responsePrefab);
            responseOption.transform.SetParent(responseContentList.gameObject.transform);
            
            responseOption.transform.localPosition = new Vector2(0, -offset);
            ResponseOption option = responseOption.GetComponent<ResponseOption>();
            option.ResponseSelected += DestroyResponses;
            offset += 45;
            string[] parts = response.Split(new string[] { "=>" }, System.StringSplitOptions.RemoveEmptyEntries);
            option.Init(parts[0], parts[1]);
            responseList.Add(responseOption);
            //UpdateOffset(1);
        }
    }

    public void GetChoices(string[] choices)
    {
        GenerateResponses(choices);
    }

    public void DestroyResponses(string s = null)
    {
        offset = 0;
        foreach (GameObject response in responseList)
            Destroy(response);
        query.gameObject.transform.localScale = Vector2.zero;
    }

    private void UpdateOffset(int responseHeight)
    {
        offset += responseHeight;
    }

    private void ActivateQueryBox()
    {
        query.gameObject.transform.localScale = Vector2.one;
    }
}
