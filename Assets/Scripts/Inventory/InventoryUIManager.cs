using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryUIManager : MonoBehaviour
{
    //The slots of the Inventory UI
    public List<RectTransform> slots;
    //The list of the Spawed UI Items in the Inventory 
    public List<GameObject> spawnedItems;
    //The Inventory panel
    public GameObject InvetoryPanel;
    //The itemUI prefab that is the blueprint for every item that we will 
    public GameObject itemUI;

    //A static bool variable so we can know from every script if we have the Invetory
    //currently opened or not
    public static bool openedInventory;
    private void Start()
    {
        //Add each slot from the InventoryPanel to the slots list
        foreach(RectTransform slot in InvetoryPanel.transform)
        {
            slots.Add(slot);
        }
        //Listen if the InventorySystem has called it's onInventoryChangedEvent and if it did call
        //this script's onUpdateUI function
        onUpdateUI();
        InventorySystem.current.onInventoryChangedEvent += onUpdateUI;
    }
    private void Update()
    {
        //Check if you pressed the I button and if you are not in a dialogue
        if (Input.GetKeyDown(KeyCode.I) && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            //Check if you have the inventory opened
            if (openedInventory)
            {
                //if you had the inventory opened deactivate the InventoryPanel and set the openedInvetory to false
                openedInventory = false;
                InvetoryPanel.SetActive(false);
            }
            else
            {
                //if not activate the InventoryPanel and set the openedInvetory to true
                openedInventory = true;
                InvetoryPanel.SetActive(true);
            }
        }
    }

    //Update the invetory UI 
    //(this function is called when the onInventoryChangedEvent is caleed in InventorySystem)
    void onUpdateUI()
    {
        //Destroy every item you spawned so far
        for(int i = 0; i < spawnedItems.Count; i++)
        {
            Destroy(spawnedItems[i]);
        }
        //Clear the list after destroying it's items
        spawnedItems.Clear();
        //Add the item's UI again
        onAddUI();
    }
    void onAddUI()
    {
        int i = 0;
        //Add an item to the Inventory UI for each item we have in the current Inventory
        foreach (InventoryItem item in InventorySystem.current.inventory)
        {
            //Check if you didn't surpass the slots available
            if (i < slots.Count)
            {
                //if you don't add the item to the Invetory UI
                AddInInventory(item, i);
                i++;
            }
            else
                break;
        }
    }

    //Add an item to the Inventory UI
    public void AddInInventory(InventoryItem item,int index)
    {
        //Spawn the prefab
        GameObject obj = Instantiate(itemUI);
        //Set the prefab's parent to the current available slot
        obj.transform.SetParent(slots[index]);
        //Set it's scale to 1 and position to 0 
        obj.GetComponent<RectTransform>().localScale = Vector3.one;
        obj.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        //Add the prefab to the spawned items list
        spawnedItems.Add(obj);
        //Get hte ItemUI component from the prefab
        ItemUI theUI = obj.GetComponent<ItemUI>();
        //Feed the prefab's InventoryItem to the ItemUI (this will help us get that item's data) 
        theUI.item = item;
        //Call the SetUI function to set the prefab's data (thing like the name, stack and icon)
        theUI.SetUI();
    }
}
