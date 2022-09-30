using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ItemUI : MonoBehaviour
{

    public Image Icon;
    public TextMeshProUGUI Name;
    public GameObject Stack;
    public TextMeshProUGUI StackNr;
    public InventoryItem item;

    //Set the Item UI to the data we got from the item variable
    public void SetUI()
    {
        //Set the icon
        Icon.sprite = item.data.icon;
        //Set the name
        Name.text = item.data.displayName;
        //Set the stack
        if(item.stackSize <= 1)
        {
            Stack.SetActive(false);
            return;
        }
        StackNr.text = item.stackSize.ToString();
    }
    //This will be called when we press on this object (this object has a button script)
    public void onConsume()
    {
        //Get the current mentalHealth value and add the value from the item variable
        int newValue = ((Ink.Runtime.IntValue)DialogueManager.GetInstance().GetVariableState("mentalHealth")).value + item.data.addHealth;

        //Set this new int vlaue to and Ink.Runtime.Object
        Ink.Runtime.Object obj = new Ink.Runtime.IntValue(newValue);

        ////Call the SetVariableState from the dialogue manager and set the 
        //mentalHealth variable the the object you created above
        DialogueManager.GetInstance().SetVariableState("mentalHealth", obj);

        //Remove the item from the inventory
        InventorySystem.current.Remove(item.data);
    }
}
