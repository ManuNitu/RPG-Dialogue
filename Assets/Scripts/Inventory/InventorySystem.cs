using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    //The InventorySystem singleton
    public static InventorySystem current { get; private set; }
    //The item dictonary where all the Item Data will be stored in the Inventory
    private Dictionary<InventoryItemData, InventoryItem> m_itemDictionary;
    //The items currently in our inventory
    public List<InventoryItem> inventory { get; private set; }
    //We delegate a ChangeAction function so that we can listen from another script if the
    //onInventoryChangedEvent has been called
    public delegate void ChangeAction();
    public event ChangeAction onInventoryChangedEvent;
    private void Awake()
    {
        //Initialize the singleton so that we can access this scripts function from any other script
        if (current == null)
        {
            current = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
        //Initialize the inventory list and the item dictonary
        inventory = new List<InventoryItem>();
        m_itemDictionary = new Dictionary<InventoryItemData, InventoryItem>();
    }

    //Add an item to the inventory 
    public void Add(InventoryItemData refrenceData)
    {
        //Check if you already have a similar item in you inventory by getting it's value
        if(m_itemDictionary.TryGetValue(refrenceData,out InventoryItem value))
        {
            //if you have one already add this new one to the stack
            value.AddToStack();
        }
        else
        {
            //if you don't add this new item to the item dictonary
            InventoryItem newItem = new InventoryItem(refrenceData);
            inventory.Add(newItem);
            m_itemDictionary.Add(refrenceData, newItem);
        }
        //Tell the InventoryUIManager script that the inventory has changed
        onInventoryChangedEvent();
    }

    //Remove an item from the inventory
    public void Remove(InventoryItemData referenceData)
    {
        //Check if you the item in you inventory by getting it's value
        if (m_itemDictionary.TryGetValue(referenceData,out InventoryItem value))
        {
            //Remove it from the stack
            value.RemoveFromStack();

            //Check if the stack hits 0
            if(value.stackSize == 0)
            {
                //if it does remove this item from the dictonary
                inventory.Remove(value);
                m_itemDictionary.Remove(referenceData);
            }
        }
        //Tell the InventoryUIManager script that the inventory has changed
        onInventoryChangedEvent();
    }
}
