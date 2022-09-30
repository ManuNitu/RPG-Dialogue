using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using Ink.Runtime;
public class DialogueManager : MonoBehaviour
{
    [Header("Load Globals JSON")]
    public TextAsset loadGlobalsJSON;

    [Header("DIALOGUE UI")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI displayNameText;
    public Animator portraitAnim;
    public Animator layoutAnim;

    [Header("CHOICES UI")]
    public GameObject[] Choices;

    [Header("ITEMS")]
    public List<InventoryItemData> itemsData;

    private TextMeshProUGUI[] choicesText;

    //The DialogueManager singleton used to acces everything from other scripts
    private static DialogueManager instance;

    private Story currentStory;

    //A static bool variable so we can know from every script 
    //if dialogue is currently playing or not
    public bool dialogueIsPlaying { get; private set; }
    private bool haveAChoice;

    //tell which speaker is which (Kevin, Granny, etc.)
    private const string SPEAKER_TAG = "speaker";
    //set the portrait (Happy, Sad, Neutral)
    private const string PORTRAIT_TAG = "portrait";
    //set the layout (left, right or item, etc.)
    private const string LAYOUT_TAG = "layout";
    //Remove an item from the player's Inventory
    private const string REMOVE_ITEM_TAG = "removeItem";
    //Add an item from the player's Inventory
    private const string GIVE_ITEM_TAG = "giveItem";

    private DialogueVariables dialogueVariables;

    //Set the singleton
    public static DialogueManager GetInstance()
    {
        return instance;
    }
    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }
        instance = this;
        dialogueVariables = new DialogueVariables(loadGlobalsJSON);
    }
    private void Start()
    {
        //Set the variables to deafault
        dialogueIsPlaying = false;
        haveAChoice = false;
        dialoguePanel.SetActive(false);
        choicesText = new TextMeshProUGUI[Choices.Length];
        int index = 0;

        //Get the text component from every choice and add it to the choicesText array
        foreach(GameObject choice in Choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }
    private void Update()
    {
        if (!dialogueIsPlaying)
            return;

        //If we pressed the mouse button and don't got a choice then we can continue the story
        //(Thats's because if we have a choice then the player can clicks the mouse wherever
        //and the dialogue will continue even if he didn't made a choice)
        if (Input.GetMouseButtonDown(0) && !haveAChoice)
            ContinueStory();

        ////If we pressed the Space key or Enter key we want to continue
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)))
        {
            ContinueStory();
        }
    }
    //Start the dialogue using the dialogue file provided
    public void EnterDialogue(TextAsset inkJSON)
    {
        //Set the currentStory to new dialogue file
        currentStory = new Story(inkJSON.text);

        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        //Start reading the dialogue file
        dialogueVariables.StartListening(currentStory);
        ContinueStory();
    }
    //Continue reading the next line in the dialogue file
    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            //Set the dialogueText to the next line in story
            dialogueText.text = currentStory.Continue();
            //Display choices if any
            DisplayChoices();
            //Handle the tags from this line in the story
            HandleStory(currentStory.currentTags);
        }
        else
        {
            ExitDialogueMode();
        }
    }
    //Check the line for tags that will help us to set some thing in the game
    private void HandleStory(List<string> currentTags)
    {
        foreach(string tag in currentTags)
        {
            //Split the current string whe you encounter the ':' charater
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
                Debug.LogError("Tag could not be apropriately parsed: " + tag);
            //The first half is the key 
            //(the const string variables from this script like SPEAKER_TAG, PORTRAIT_TAG, etc.)
            string tagKey = splitTag[0].Trim();
            //The second half is the value
            string tagValue = splitTag[1].Trim();

            //For example when we write "#speaker: Kevin" in the Ink file
            //this function will recognize the key(speaker) and it's value(Kevin)
            //and when the dialogue is played the name text will be set to the 
            //value of Kevin

            switch (tagKey)
            {
                //Set the speaker
                case SPEAKER_TAG:
                    displayNameText.text = tagValue;
                    break;
                //Set the portrait of the speaker
                case PORTRAIT_TAG:
                    portraitAnim.Play(tagValue);
                    break;
                //Set the layout
                case LAYOUT_TAG:
                    layoutAnim.Play(tagValue);
                    break;
                //Give an item
                case GIVE_ITEM_TAG:
                    InventoryItemData addItem = itemsData.Find(x => x.displayName == tagValue);
                    InventorySystem.current.Add(addItem);
                    break;
                //Remove an item
                case REMOVE_ITEM_TAG:
                    InventoryItem removeItem = InventorySystem.current.inventory.Find(x => x.data.displayName == tagValue);
                    InventorySystem.current.Remove(removeItem.data);
                    break;
                //Stop the game because the tag doesn't exist
                default:
                    Debug.LogError("Tag came in but is not currently handled: " + tag);
                    break;
            }
        }

    }
    //Make the choices visible
    public void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        //Check if we have a choice to make
        if (currentStory.currentChoices.Count > 0)
            haveAChoice = true;

        //Check if the Ink file has more choice that we can handle
        if(currentChoices.Count > Choices.Length)
        {
            //if it does stop the game with an error
            Debug.LogError("More choices were give that the UI can support. Number of choices given: " + currentChoices.Count);
        }

        int index = 0;
        //Foreach choice in this part of the file set it's text and enable the choice UI buttons
        foreach(Choice choice in currentChoices)
        {
            Choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        //If we want to have less choices for some dialogues
        //for example 2 choice not 3 which we usually provide
        //In this case we want to disable last choice in this case because we don't need it
        for (int i = index; i < Choices.Length; i++)
        {
            Choices[i].SetActive(false);
        }
        //Use the eventSystem to select the first choice when the choice appear
        //This way we can use the arrow keys to navigate the choices
        StartCoroutine("SelectFirstChoice");
    }

    //Disable everything dialogue related
    private void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueVariables.StopListening(currentStory);
        dialogueText.text = "";
    }

    //Use the eventSystem to select the first choice when the choice appear
    //This way we can use the arrow keys to navigate the choices
    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(Choices[0].gameObject);
    }

    //The function called when you pressed a choice button
    public void MakeChoice(int choiceIndex)
    {
        Debug.Log("PRESSED");
        currentStory.ChooseChoiceIndex(choiceIndex);
        haveAChoice = false;
    }

    //The function used to change the a varible's values in the Global Ink file
    public void SetVariableState(string variableName, Ink.Runtime.Object variableValue)
    {
        if (dialogueVariables.variables.ContainsKey(variableName))
        {
            dialogueVariables.variables.Remove(variableName);
            dialogueVariables.variables.Add(variableName, variableValue);
        }
        else
        {
            Debug.LogWarning("Tried to update variable that wasn't initialized by globals.ink: " + variableName);
        }
    }

    //The function used to get the a varible's values in the Global Ink file
    public Ink.Runtime.Object GetVariableState(string variableName)
    {
        Ink.Runtime.Object variableValue = null;
        dialogueVariables.variables.TryGetValue(variableName, out variableValue);
        if(variableValue == null)
        {
            Debug.LogWarning("Ink Variable was found to be null: " + variableName);
        }
        return variableValue;
    }
}
