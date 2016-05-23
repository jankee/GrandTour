using UnityEngine;
using System.Collections;

public class Item
{
    public string itemName;
    public int itemID;
    public string itemDescription;
    public Sprite itemIcon;
    public GameObject itemModel;
    public int itemPower;
    public int itemSpeed;
    public int itemVelue;
    public ItemType itemtype;

    public enum ItemType
    {
        Weapon,
        Consumable,
        Quest,
        Head,
        Shoes,
        Chest,
        Trousers,
        Earrings,
        Necklace,
        Ring,
        Hands
    }

    // Use this for initialization
    public Item(string name, int id, string desc, int powor, int speed, int value, ItemType type)
    {
        itemName = name;
        itemID = id;
        itemDescription = desc;
        itemPower = powor;
        itemSpeed = speed;
        itemVelue = value;
        itemtype = type;
        itemIcon = Resources.Load<Sprite>("" + name);
    }

    public Item()
    {

    }
}
