using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem
{
    //A public class that handles the stack(number) of the current Item 

    public InventoryItemData data { get; private set; }
    public int stackSize { get; private set; }
    public InventoryItem(InventoryItemData source)
    {
        data = source;
        AddToStack();
    }
    public void AddToStack()
    {
        stackSize++;
    }
    public void RemoveFromStack()
    {
        stackSize--;
    }
}
