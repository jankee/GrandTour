using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class SlotScript : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{
    public Item item;

    Image itemImage;
    //Inventory에서 사용할 스롯넘버
    public int slotNumber;

    Inventory inventory;

    // Use this for initialization
    void Start()
    {
        //인벤토리 찾아 초기화 한다.
        inventory = GameObject.FindGameObjectWithTag("INVENTORY").GetComponent<Inventory>();

        itemImage = gameObject.transform.GetChild(0).GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inventory.Items[slotNumber].itemName != null)
        {
            itemImage.enabled = true;
            itemImage.sprite = inventory.Items[slotNumber].itemIcon;
        }
        else
        {
            itemImage.enabled = false;
        }
	}

    public void OnPointerDown(PointerEventData data)
    {
        print("Click");
    }

    public void OnPointerEnter(PointerEventData data)
    {
        print(gameObject.name);
    }
}
