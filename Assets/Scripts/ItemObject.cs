using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    //The text file that we will run when we pick up the item
    [Header("INK JSON")]
    public TextAsset inkJSON;

    //Get the InventoryItemData reference to know this item's values (name, cost, icon, etc)
    public InventoryItemData referenceItem;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if the player has entered this object's Trigger
        if (collision.CompareTag("Player"))
        {
            //Call the onHandlePickUp
            onHandlePickUp();

            //Check if we are not in a dialogue 
            if (!DialogueManager.GetInstance().dialogueIsPlaying)
            {
                //Declare a Ink.Runtime.Object and put the referenceItem.displayName as a string value
                Ink.Runtime.Object obj = new Ink.Runtime.StringValue(referenceItem.displayName);

                //Call the SetVariableState from the dialogue manager and set the 
                //itemPicked variable the the object you created above
                DialogueManager.GetInstance().SetVariableState("itemPicked", obj);

                //Enter the dialogue after you chaneg the itemPicked variable
                DialogueManager.GetInstance().EnterDialogue(inkJSON);
            }
        }
    }

    //Add the item to the inventory and destroy it from the game
    public void onHandlePickUp()
    {
        InventorySystem.current.Add(referenceItem);
        Destroy(this.gameObject);
    }
}
