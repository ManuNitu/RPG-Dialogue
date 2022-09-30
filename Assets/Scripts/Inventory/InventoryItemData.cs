using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Inventory Item Data")]
public class InventoryItemData : ScriptableObject
{
    //An scriptable object class that let's us create in the inspector object with these values
    public int addHealth;
    public int cost;
    public string id;
    public string displayName;
    public Sprite icon;
    public GameObject prefab;
}
