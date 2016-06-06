using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDatabase : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    // Use this for initialization
    void Start()
    {
        //items.Add(new Item("A_Armor04", 0, "Nice Armor", 10, 10, 1, Item.ItemType.Chest));
        //items.Add(new Item("A_Armor05", 1, "Better Armor", 10, 10, 1, Item.ItemType.Chest));
        //items.Add(new Item("I_Antidote", 2, "Nice Consumable", 10, 10, 1, Item.ItemType.Consumable));
    }
}
