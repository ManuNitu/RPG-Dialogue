using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("VISUAL CUE")]
    public GameObject visualCue;
    [Header("INK JSON")]
    public TextAsset inkJSON;
    public string npcName;
    [HideInInspector]
    public bool playerInRange;
    private void Awake()
    {
        playerInRange = false;
    }
    private void Update()
    {
        //Check if the player is in range, if a dialogue is not playing and
        //if our inventory is not currently opened
        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying && !InventoryUIManager.openedInventory)
        {
            visualCue.SetActive(true);
            //If the player presses E
            if (Input.GetKeyDown(KeyCode.E))
            {
                //Check if the Quest variable for this NPC exists
                //(this will exist if the NPC has a quest)
                if (DialogueManager.GetInstance().GetVariableState(npcName + "Quest") != null)
                {
                    //Check if this NPC's quest is active 
                    //by checking if it's Quest variable value is 1
                    //(If the Quest is not active it will have the value 0)
                    if (((Ink.Runtime.IntValue)DialogueManager.GetInstance().GetVariableState(npcName + "Quest")).value == 1)
                    {
                        //Get the QuestItemNeeded from the Ink file 
                        //and put it in a string called requiredItem 
                        string requiredItem = ((Ink.Runtime.StringValue)DialogueManager.GetInstance().GetVariableState(npcName + "QuestItemNeeded")).value;

                        //Check each item in your inventory if it is the requiredItem
                        foreach (InventoryItem item in InventorySystem.current.inventory)
                        {
                            if (item.data.displayName == requiredItem)
                            {
                                //if it is set the HaveQuestItem variable to 1 and exit the foreach loop
                                Ink.Runtime.Object obj = new Ink.Runtime.IntValue(1);
                                DialogueManager.GetInstance().SetVariableState(npcName + "HaveQuestItem", obj);
                                break;
                            }
                            else
                            {
                                //if it's not set the HaveQuestItem variable to 0 and keep searching
                                Ink.Runtime.Object obj = new Ink.Runtime.IntValue(0);
                                DialogueManager.GetInstance().SetVariableState(npcName + "HaveQuestItem", obj);
                            }
                        }
                    }
                }
                //Enter the dialogue
                DialogueManager.GetInstance().EnterDialogue(inkJSON);
            }
        }
        else
            visualCue.SetActive(false);
       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if the player has entered this object's Trigger
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //Check if the player has exitted this object's Trigger
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}

