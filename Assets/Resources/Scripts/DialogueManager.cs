using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public delegate void DialogueChange(string dialogue);
public delegate void GivePlayerChoice(string[] choices);
public delegate void OnResponseSelect(string tag);
public delegate void OnSpeakerChange(CharacterProfile profile);
public delegate void OnQueryKeywordsDetected();
public delegate void DialogueEventHandler(string message);
public delegate void ConversationStateChangeHandler(bool state);
public delegate void AdvanceQuestHandler(string questName);

public class DialogueManager : MonoBehaviour
{
    private const string CONTINUE = "CONTINUE";
    private const string FINISH = "FINISH";
    private readonly string[] SEPARATOR = new string[]{"=>"};
    private readonly char[] SEPARATOR_UNDERSCORE = new char[] { '_' };
    bool continueReadingLines = false;
    private static DialogueManager dialogueManager;
    private string[] dialogue;
    private Dictionary<string, int> tagMap;
    private Dictionary<string, string> keywordQueryMap;
    private int currLine = 0;
    public bool executing = false;

    //Delgates
    public DialogueChange DialogueEvent;
    public GivePlayerChoice GivePlayerChoices;
    public OnSpeakerChange SpeakerChanged;
    public OnQueryKeywordsDetected QueryDetected;
    public DialogueEventHandler DialogueEventRaised;
    public ConversationStateChangeHandler ConversationStateChanged;
    public AdvanceQuestHandler QuestAdvanced;

    private Dictionary<string, CharacterProfile> characterMap;
    [SerializeField]
    private List<CharacterProfile> characters;
    [SerializeField]
    private CharacterProfile speakingCharacter;
    private ResponseBox playerChoiceBox;
    private DialogueBox dialogueBox;
    public GameObject DialogueBoxPrefab;
    public GameObject PlayerResponseBoxPrefab;

    public static DialogueManager Instance
    {
        get
        {
            if (dialogueManager == null)
            {
                dialogueManager = FindObjectOfType<DialogueManager>();
            }
            return dialogueManager;
        }
    }

    // Start is called before the first frame update
    void Awake() 
    {
        dialogueManager = this;
        
        keywordQueryMap = new Dictionary<string, string>();
    }

    void Start()
    {
        InitCharacterMap();
        //Get each CharacterSO being used in the scene and add it to the characters dictionary
    }

    //Have a conversation!
    public void BeginConversation(DialogueSO dialogue)
    {
        ParseDialogueFile(dialogue.dialogueFile);
  
        executing = true;
        currLine = 0;
        tagMap.TryGetValue("entry", out currLine);

        if (!dialogueBox)
        {
            //Check if exists with FindObjectOfType
            dialogueBox = FindObjectOfType<DialogueBox>();
            playerChoiceBox = FindObjectOfType<ResponseBox>();
            if (!dialogueBox)
            {
                //If not, Instantiate 
                CreateUIComponents();
            }
        }
            
        else
        {
            dialogueBox.gameObject.SetActive(true);
            playerChoiceBox.gameObject.SetActive(true);
        }

        ConversationStateChanged?.Invoke(true);
        Read();
    }

    private void CreateUIComponents()
    {
        GameObject go = FindObjectOfType<Canvas>().gameObject;
        dialogueBox = Instantiate(DialogueBoxPrefab).GetComponent<DialogueBox>();
        playerChoiceBox = Instantiate(PlayerResponseBoxPrefab).GetComponent<ResponseBox>();

        dialogueBox.gameObject.transform.SetParent(go.transform, false);
        playerChoiceBox.gameObject.transform.SetParent(go.transform, false);
    }

    //End the conversation.
    //NOTE: When called, does not prevent executing functions from finishing up. So a line is likely to be returned after calling on this.
    private void EndConversation()
    {
        currLine = 0;
        executing = false;
        dialogueBox.gameObject.SetActive(false);
        playerChoiceBox.gameObject.SetActive(false);
        ConversationStateChanged?.Invoke(false);
    }

    public void Read()
    {
        if (executing)
        {
            string s = ReadDialogue();
            if (s != null)
            {
                DialogueEvent?.Invoke(s);
            }
        }
    }

    //Get the next viable string of dialogue and return it.
    public string ReadDialogue()
    {
        NextLine();

        //TODO: Change to avoid getting blank dialogueBoxes outputting null
        if (!executing)
            return null;

        //Check the viability of the next line.
        string s = ProcessLine(dialogue[currLine]);

        //If non-viable, try to find the next viable line
        //TODO: Replace null with something else that won't cause errors
        if (s.Equals(CONTINUE))
            return executing ? ReadDialogue() : null;
        else if (s[0] == '*')
            return null;
        else
        {
            if (executing)
            {
                //Check if the line has a link associated with it. If so, follow it. If not, return the line as normal.
                //2 parts indicates that the Separator symbol was within the string
                string[] parts = s.Split(SEPARATOR, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2)
                    GoToTag(parts[1]);
                return executing ? parts[0] : null;
            }
            else
                return null;
            //return executing ? ProcessLine(dialogue[currLine]) : null;
        }
    }

    //Go to the index associated with the provided tag
    public void GoToTag(string tag)
    {
        DialogueEventRaised?.Invoke(tag);
        //Get the speaker's name from the tag
        string speakerName = tag
            .Split(SEPARATOR_UNDERSCORE, StringSplitOptions.RemoveEmptyEntries)[0]
            .ToLower();

        var character = GetCharacterProfileByName(speakerName);
        
        SpeakerChanged?.Invoke(character);

        //If the tag is the keyword FINISH, then end the conversation
        if (tag.Equals(FINISH))
        {
            EndConversation();
            return;
        }
        if (tagMap.ContainsKey(tag))
        {
            currLine = tagMap[tag];
            //NextLine();
        }
    }


    //Go to the next line of dialogue.
    //Evaluate and responses necessary until we arrive at a line of dialogue.
    private void NextLine()
    {
        currLine++;

        if (!executing || currLine >= dialogue.Length)
        {
            executing = false;
            return;
        }

        if (dialogue[currLine][0] == '#')
            currLine++;

        //If reading lines of NPC dialogue
        if (dialogue[currLine].Equals("*D*"))
            FlagD();
        //If reading lines of responses
        else if (dialogue[currLine].Equals("*R*"))
        {
            FlagR();
            return;
        }
        //If reading lines of player dialogue
        else if (dialogue[currLine].Equals("*C*"))
            FlagC();
        else if (dialogue[currLine].Equals("*Q*"))
            FlagQ();
        else if (dialogue[currLine] == "*BEGIN*")
            continueReadingLines = true;
        else if (dialogue[currLine] == "*END*")
            continueReadingLines = false;

        if (continueReadingLines)
            NextLine();

        void FlagD()
        {
            currLine++;
        }

        void FlagR()
        {
            //Process response conditions until one passes
            //NOTE: AT LEAST ONE CONDITION WILL ALWAYS PASS
            bool passed = false;
            while (!passed)
            {
                currLine++;
                string tag = ProcessResponse(dialogue[currLine]);
                if (tag != null)
                {
                    passed = true;
                    GoToTag(tag);
                    NextLine();
                }
            }
        }

        void FlagC()
        {
            currLine++;
            GivePlayerChoices?.Invoke(ProcessPlayerChoices(GetChoices()));
        }

        void FlagQ()
        {
            currLine++;
            keywordQueryMap.Clear();
            while (!dialogue[currLine].Equals("*/Q*"))
            {
                string[] parts = dialogue[currLine].Split(SEPARATOR, StringSplitOptions.RemoveEmptyEntries);
                keywordQueryMap.Add(parts[0].ToLower(), parts[1]);
                currLine++;
            }

            //If there are query keywords, then ignite the event to tell the response box to display the query box
            if (keywordQueryMap.Count > 0)
                QueryDetected();
        }

    }


    //Read player dialogue choices
    private string[] GetChoices()
    {
        List<string> choices = new List<string>();
        while (!dialogue[currLine].Equals("*/C*"))
        {
            choices.Add(dialogue[currLine]);
            currLine++;
        }

        return choices.ToArray();
    }

    //Process a line of dialogue for any conditions
    private string ProcessLine(string line)
    {
        bool flagMode = false;
        bool conditionalMode = false;
        string flag = "";
        string result = "";
        string condition = "";

        //If the seperator was absent, then process normally
        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];
            if (c == '(')
                conditionalMode = true;
            else if (c == ')')
            {
                conditionalMode = false;
                string[] cond = condition.Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries);
                condition = cond[0];
                string[] args = new string[cond.Length - 1];
                Array.Copy(cond, 1, args, 0, args.Length);

                //If a condition does not pass. then immediately proceed to the next line
                if (!AddressBook.FindAddress(condition, args))
                {
                    return CONTINUE;
                }
                condition = "";
            }
            else if (c == '{')
                flagMode = true;
            else if (c == '}')
            {
                flagMode = false;
                string s = (string)AddressBook.FindValue(flag);
                result += s;
                flag = "";
            }
            else            
            {
                if (flagMode)
                    flag += c;
                else if (conditionalMode)
                    condition += c;
                else
                    result += c;
            }
        }
        return result;
    }

    //Process a response for its conditions and its link
    private string ProcessResponse(string responseLine)
    {
        bool accepted = true;
        string[] parts = responseLine.Split(SEPARATOR, StringSplitOptions.RemoveEmptyEntries);
        string responseTag = parts[1];
        //Each unique condition is contained in a set of parentheses.
        string[] conditions = parts[0].Split(new char[]{'(', ')'}, StringSplitOptions.RemoveEmptyEntries);
        
        foreach (string condition in conditions)
        {
            string[] args = condition.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string actionName = args[0];
            string[] args2 = new string[args.Length - 1];
            Array.Copy(args, 1, args2, 0, args2.Length);

            accepted = AddressBook.FindAddress(actionName, args2);
            Debug.Log(actionName + " " + args2);
            if (!accepted)
                break;
        }

        if (accepted)
            return responseTag;
        
        return null;
    }

    private string[] ProcessPlayerChoices(string[] choices)
    {
        List<string> result = new List<string>();

        foreach(string choice in choices)
        {
            //Evaluate conditions
            //If all pass, then add the string to the List of choices
            string r = ProcessLine(choice);
            if (r.Equals(CONTINUE))
                continue;
            result.Add(r);
        }

        return result.ToArray();
    }

    //Construct the tagMap using a dialogue file
    private void ParseDialogueFile(TextAsset ta)
    {
        dialogue = ta.text.Split(new char[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);

        tagMap = new Dictionary<string, int>();
        //Take an account of every tag in the file
        for (int i = 0; i < dialogue.Length; i++)
        {
            string line = dialogue[i];
            //Debug.Log(line);   
            if (line.Equals(""))
                continue;
            //All lines beginning with the '#' character are tags
            if (line[0] == '#')
            {
                string tag = line.Substring(1);
                tagMap.Add(tag.Trim(), i);                
            }
        }
    }

    public void GoToTagAndRead(string tag)
    {
        GoToTag(tag);
        Read();
    }

    public void UpdateCharacterProfile(CharacterProfile profile)
    {
        speakingCharacter = profile;
    }

    private void InitCharacterMap()
    {
        characterMap = new Dictionary<string, CharacterProfile>();
        foreach(CharacterProfile profile in characters)
        {
            characterMap.Add(profile.characterName
                .Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[0]
                .ToLower(), profile);
        }
    }

    public CharacterProfile GetCharacterProfileByName(string name)
    {
        if (characterMap.ContainsKey(name))
            return characterMap[name];
        else
            return null;
    }

    public string ProcessQuery(string query)
    {
        if (keywordQueryMap.ContainsKey(query))
        {
            return keywordQueryMap[query];
        }

        return null;
    }

    public void QuerySubmitted(string query)
    {
        string tag = ProcessQuery(query.ToLower());
        if (tag != null)
        {
            GoToTag(tag);
            Read();
        }
        else
            //Fire event for bad query
            return;
    }

    //When reading some lines, check if the string is equal to one of the reserved words.
    //In that event, perform some action
    private void EventLookup(string query, string[] args)
    {
        switch (query)
        {
            case "ADVANCEQUEST":
                QuestAdvanced?.Invoke(args[1]);
                break;

            default: return;
        }
    }

}