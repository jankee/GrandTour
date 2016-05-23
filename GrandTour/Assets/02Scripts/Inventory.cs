using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public List<GameObject> Slots = new List<GameObject>();
    public List<Item> Items = new List<Item>();
    public GameObject slots;

    ItemDatabase database;

    int x = -110;
    int y = 118;

    // Use this for initialization
    void Start()
    {
        int slotAmout = 0;

        database = GameObject.FindGameObjectWithTag("ITEMDATABASE").GetComponent<ItemDatabase>();

        for (int i = 0; i < 5; i++)
        {
            for (int k = 0; k < 5; k++)
            {

                GameObject slot = (GameObject)Instantiate(slots);

                slot.GetComponent<SlotScript>().slotNumber = slotAmout;

                Slots.Add(slot);

                //이해하지 못하는 곳
                //Items.Add(database.items[i]);
                Items.Add(new Item());


                slot.transform.parent = this.gameObject.transform;
                slot.name = "slot" + i + "-" + k;
                slot.GetComponent<RectTransform>().localPosition = new Vector3(x, y, 0);
                x = x + 55;

                if (k == 4)
                {
                    x = -110;
                    y = y - 55;
                }
                //slotAmount에 가산을 한다.
                slotAmout++;
            }
        }

        AddItem(0);
        AddItem(1);

        print(Items[0].itemName);
        print(Items[1].itemName);
    }
    
    void AddItem(int id)
    {
        for (int i = 0; i < database.items.Count; i++)
        {
            if (database.items[i].itemID == id)
            {
                Item item = database.items[i];
                AddItemAtEmptySlot(item);

                break;
            }
        }
    }

    void AddItemAtEmptySlot(Item item)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].itemName == null)
            {
                Items[i] = item;

                break;
            }
        }
    }
}
