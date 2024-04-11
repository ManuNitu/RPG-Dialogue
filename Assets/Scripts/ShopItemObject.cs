using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemObject : MonoBehaviour
{
    [Header("INK JSON")]
    public TextAsset inkJSON;

    public InventoryItemData referenceItem;
    public GameObject visualCue;

    private bool playerInRange;

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
                //Declare a Ink.Runtime.Object and put the referenceItem.displayName as a string value
                Ink.Runtime.Object obj = new Ink.Runtime.StringValue(referenceItem.displayName);

                //Call the SetVariableState from the dialogue manager and set the 
                //itemToBuy variable the the object you created above
                DialogueManager.GetInstance().SetVariableState("itemToBuy", obj);

                //Now set the Ink.Runtime.Object to the referenceItem.cost as an int value
                obj = new Ink.Runtime.IntValue(referenceItem.cost);

                //Call the SetVariableState from the dialogue manager and set the 
                //itenToBuyCost variable the the object you created above
                DialogueManager.GetInstance().SetVariableState("itemToBuyCost", obj);

                //Enter the dialogue
                DialogueManager.GetInstance().EnterDialogue(inkJSON);
            }
        }
        else
            visualCue.SetActive(false);

        //Check if the player is in range and if the itemPicked variable has a value
        //If this is true that means we have some leftovers from out previous interactions
        //For example maybe we wanted to buy an item and didn't have enough coins
        //We didn't got the item but the variables itemPicked, itemToBuy and itemToBuyCost
        //were already set to that objects values
        //So we need to reset all of thos the their default value ("" for strings and 0 for ints)
        if ( playerInRange &&((Ink.Runtime.StringValue)DialogueManager.GetInstance().GetVariableState("itemPicked")).value != "")
        {
            Ink.Runtime.Object obj = new Ink.Runtime.StringValue("");
            DialogueManager.GetInstance().SetVariableState("itemPicked", obj);

            DialogueManager.GetInstance().SetVariableState("itemToBuy", obj);

            obj = new Ink.Runtime.IntValue(0);
            DialogueManager.GetInstance().SetVariableState("itemToBuyCost", obj);

            onHandlePickUp();
        }
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

    //Add the item to the inventory and destroy it from the game
    public void onHandlePickUp()
    {
        InventorySystem.current.Add(referenceItem);
        Destroy(this.gameObject);
    }
}
