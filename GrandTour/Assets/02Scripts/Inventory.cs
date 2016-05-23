using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    //인벤토리 RectTransform 컴포넌트 변수
    private RectTransform inventoryRect;
    //인벤토리의 가로,세로 길이
    private float inventoryWidth, inventoryHight;
    //인벤토리의 슬롯갯수
    public int slots;
    //인벤토리의 로우갯수
    public int rows;
    //슬롯의 옆, 위 간격 거리
    public float slotPaddingLeft, slotPaddingTop;
    //슬롯의 크기
    public float slotSize;
    //슬롯의 프립펩
    public GameObject slotPrefab;
    //슬롯 리스트 변수
    private List<GameObject> allSlots;

    // Use this for initialization
    void Start()
    {
        CreateLayout();
    }

    void Update()
    {

    }

    private void CreateLayout()
    {
        //allSlots 리스트를 초기화
        allSlots = new List<GameObject>();
        //인벤토리 가로 길이 계산
        inventoryWidth = (slots / rows) * (slotSize + slotPaddingLeft) + slotPaddingLeft;
        //인벤토리 세로 길이 계산
        inventoryHight = rows * (slotSize + slotPaddingTop) + slotPaddingTop;

        //inventoryRect의 RectTransform의 초기화
        inventoryRect = GetComponent<RectTransform>();
        //inventoryRect의 가로길이에 inventoryWidth값을 넣어 준다
        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, inventoryWidth);
        //inventoryRect의 세로길이에 inventoryHight값을 넣어 준다
        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, inventoryHight);

        //-------------------------스롯 위치 계산----------------------------------------
        //컬럼의 갯수를 계산
        int colums = slots / rows;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < colums; x++)
            {
                //newSlot 변수에 slotPrefab을 생성해 준다
                GameObject newSlot = (GameObject)Instantiate(slotPrefab);
                //slotRect의 newSlot의 RectTransform 컴포넌트 변수를 넣어 준다
                RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                //생성된 newSlot에 이름을 Slot으로 정한다.
                newSlot.name = "Slot";
                //newSlot의 부모를 Canvas로 정해 준다
                newSlot.transform.SetParent(this.transform.parent);
            }
        }
    }
    
}
