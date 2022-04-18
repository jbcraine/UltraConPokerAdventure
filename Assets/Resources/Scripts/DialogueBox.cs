using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    public string text;
    public TMP_Text textMesh;
    public Image profilePicture;
    public string speakerName;
    public Color textColor;
    public AudioSource textSound;
    public AudioClip textClip;

    // Start is called before the first frame update
    void Awake()
    {
        textSound = GetComponent<AudioSource>();
    }

    private void Start()
    {

    }
    private void OnEnable()
    {
        DialogueManager.Instance.DialogueEvent += UpdateText;
        DialogueManager.Instance.SpeakerChanged += UpdateSpeaker;
        DialogueBoxButton button = GetComponentInChildren<DialogueBoxButton>();
        button.PointerDown += DialogueManager.Instance.Read;
    }

    private void OnDisable()
    {
        DialogueBoxButton button = GetComponentInChildren<DialogueBoxButton>();
        button.PointerDown -= DialogueManager.Instance.Read;
        DialogueManager.Instance.DialogueEvent -= UpdateText;
        DialogueManager.Instance.SpeakerChanged -= UpdateSpeaker;
    }


    public void UpdateText(string text)
    {
        StopAllCoroutines();
        text = ProcessText(text);
        this.text = text;
        textMesh.text = string.Empty;
        StartCoroutine(Typewriter(text));
    }

    public void UpdateSpeaker(CharacterProfile character)
    {
        //Debug.Log(character);
        if (character == null)
        {
            profilePicture.gameObject.SetActive(false);
            textColor = Color.white;
            speakerName = "";
            profilePicture.sprite = null;
            textClip = null;
            textSound.clip = null;
            return;
        }

        profilePicture.gameObject.SetActive(true);
        profilePicture.sprite = character.profileImage;
        textColor = character.textColor;
        textMesh.color = textColor;
        speakerName = character.characterName;
        textClip = character.speechSound;
        textSound.clip = textClip;
    }

    public string ProcessText(string line)
    {
        bool variableMode = false;
        string result = "";
        string variableName = "";
        foreach (char c in line)
        {
            if (c == '{')
                variableMode = true;
            else if (c == '}')
            {
                variableMode = false;
                object o;
                if ((o = AddressBook.FindValue(variableName)) != null)
                    result += o.ToString();
            }

            if (variableMode)
                variableName += c;
            else
                result += c;
        }

        return result;

    }

    private IEnumerator Typewriter(string line)
    {
        foreach (char c in line)
        {
            textMesh.text += c;
            textSound?.Play();
            yield return new WaitForSeconds(0.05f);
        }
    }


}
