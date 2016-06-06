using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    //스탁타입의 items 변수 선언 초기화
    private Stack<Item> items;

    public Stack<Item> Items
    {
        get { return items; }
        set { items = value; }
    }

    //스롯 넘버를 표시할 Text변수
    public Text stackTxt;
    //스롯 스프라이트 변수
    public Sprite slotEmpty;
    public Sprite slotHighlight;
    //스롯이 비여있는 알수 있는 변수
    public bool IsEmpty
    {
        get
        {
            //items의 카운트가 0이면 참을 반환한다
            return items.Count == 0;
        }
    }

    public bool IsAvailable
    {
        get
        {
            print(CurrentItem.maxSize + " - " + items.Count);
            return CurrentItem.maxSize > items.Count;
        }
    }

    public Item CurrentItem
    {
        get
        {
            return items.Peek();
        }
    }


    // Use this for initialization
    void Start()
    {
        //변수 초기화
        items = new Stack<Item>();
        //렉트 트렌스폼 로컬 변수로 초기화
        RectTransform slotRect = GetComponent<RectTransform>();
        //stackTxt 컴포넌트를 RectTransform으로 적용
        RectTransform txtRect = stackTxt.GetComponent<RectTransform>();
        //스롯 사이즈의 값의 60%를 int타입으로 변경하여 지역변수로 저장
        int txtScaleFactor = (int)(slotRect.sizeDelta.x * 0.6f);
        //stackTxt의 Max, Min 사이즈를 txtScaleFactor로 정해준다.
        stackTxt.resizeTextMaxSize = txtScaleFactor;
        stackTxt.resizeTextMinSize = txtScaleFactor;

        //stackTxt의 크기를 txtScaleFactor 값으로 변경해준다
        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotRect.sizeDelta.x);
        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotRect.sizeDelta.y);

        GameObject mana = GameObject.Find("Mana");

        //AddItem(mana.GetComponent<Item>());
    }

    // Update is called once per frame
    void Update()
    {

    }

    //items 스탁에 item에 넣어 준는 함수
    public void AddItem(Item item)
    {
        //아이템에 푸쉬해준다
        items.Push(item);

        print(items.Peek());
        //stackTxt.Text의 items의 카운트를 숫자를 넣어 준다
        if (items.Count > 1)
        {
            stackTxt.text = items.Count.ToString();    
        }
        //푸쉬된 아이템의 스프라이트를 ChangeSprite 함수에 넣어 준다
        ChangeSprite(item.spriteNeutral, item.spriteHighlighted);
        
    }

    public void AddItems(Stack<Item> items)
    {
        this.items = new Stack<Item>(items);

        stackTxt.text = items.Count > 1 ? items.Count.ToString() : string.Empty;

        ChangeSprite(CurrentItem.spriteNeutral, CurrentItem.spriteHighlighted);
    }

    //상태에 따라 스프라이트 이미지를 교체 해 주는 함수
    private void ChangeSprite(Sprite neutral, Sprite highlight)
    {
        this.GetComponent<Image>().sprite = neutral;

        //스프라이트 상태를 st 지역변수로 정한다
        SpriteState st = new SpriteState();
        //st 변수의 highlightedSprite, pressedSprite에 이미지를 넣어준다
        st.highlightedSprite = highlight;
        st.pressedSprite = neutral;
        //버튼에 spriteState에 st 변수를 넣어 준다
        this.GetComponent<Button>().spriteState = st;
    }

    //아이템을 사용하는 함수
    private void UseItem()
    {
        if (!IsEmpty)
        {
            items.Pop().Use();

            //items의 카운트가 1보다 크면 숫자를 써 주고 아니면 비워둔다
            stackTxt.text = items.Count > 1 ? items.Count.ToString() : string.Empty;
            //비여있다면
            if (IsEmpty)
            {
                //스롯용 스프라이트를 넣어 준다
                ChangeSprite(slotEmpty, slotHighlight);
                //인벤토리 클레스의 emptySlot에 하나를 더 해준다.
                Inventory.EmptySlot++;

            }
        }
    }

    //클리어 슬롯 함수
    public void ClearSlot()
    {
        //items를 클리어 한다
        items.Clear();
        //ChangSprite 함수로 slotEmpty, slotHighlight 슬롯을 넣어준다
        ChangeSprite(slotEmpty, slotHighlight);
        //슬롯 갯수 텍스트를 0으로 만들어 준다
        stackTxt.text = string.Empty;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        //마우스 오른쪽 클리과 Hover 아이콘이 없고 인벤토리그릅의 알파가 0보다 크면
        if (eventData.button == PointerEventData.InputButton.Right && !GameObject.Find("Hover")
            && Inventory.CanvasGroup.alpha > 0)
        {
            UseItem();
        }
    }
}
