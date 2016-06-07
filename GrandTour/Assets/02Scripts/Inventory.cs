using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
    //
    private static Slot from, to;
    //슬롯 리스트 변수
    private List<GameObject> allSlots;

    public GameObject iconPrefab;

    public EventSystem eventSystem;

    private static GameObject hoverObject;

    public Canvas canvas;

    private static CanvasGroup canvasGroup;

    public static CanvasGroup CanvasGroup
    {
        get { return canvasGroup; }
    }

    private static Inventory instance;

    public static Inventory Instance
    {

        get 
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Inventory>();
            }
            return Inventory.instance; 
        }
    }

    private bool fadingIn, fadingOut;

    public float fadingTime;

    private float hoverYOffset;

    //비여있는 슬롯 확인 변수
    private static int emptySlot;
    //emptySlot 캡슐화
    public static int EmptySlot
    {
        get { return emptySlot; }
        set { emptySlot = value; }
    }

    private static GameObject clicked;

    public GameObject selectStackSize;

    public Text stackText;


    private int splitAmount;

    private int maxStackCount;

    private static Slot movingSlot;
         
    // Use this for initialization
    void Start()
    {
        canvasGroup = transform.parent.GetComponent<CanvasGroup>();

        //레이아웃 생성 함수 호출
        CreateLayout();

        movingSlot = GameObject.Find("MovingSlot").GetComponent<Slot>();
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            //EventSystem 밖에서 마우스를 사용했을 때
            if (!eventSystem.IsPointerOverGameObject(-1) && from != null)
            {
                //from 슬롯을 다시 화이트로 돌려주고 hover를 지워준다
                from.GetComponent<Image>().color = Color.white;
                from.ClearSlot();
                Destroy(GameObject.Find("Hover"));
                to = null;
                from = null;
                hoverObject = null;
                //emptySlot에 하나를 더 해준다
                emptySlot++;
            }
        }

        if (hoverObject != null)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out position);
            //hoverObject의 위치값을 내려서 위치시킨다.
            position.Set(position.x, position.y - hoverYOffset);
            hoverObject.transform.position = canvas.transform.TransformPoint(position);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (canvasGroup.alpha > 0)
            {
                StartCoroutine("FadeOut");
                //인벤토리가 안보여지면 원위치를 시킨다
                PutItemBack();
            }
            else
            {
                StartCoroutine("FadeIn");
            }
        }
    }

    private void CreateLayout()
    {
        //allSlots 리스트를 초기화
        allSlots = new List<GameObject>();

        hoverYOffset = slotSize * 0.01f;
        //slots의 갯수를 emptySlot에 넣어 준다
        emptySlot = slots;
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
                //스롯의 포지션을 정한다.
                slotRect.localPosition = inventoryRect.localPosition + new Vector3(slotPaddingLeft * (x + 1) + (slotSize * x), 
                    -slotPaddingTop * (y + 1) - (slotSize * y), 0);
                //스롯의 사이즈 변경
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * canvas.scaleFactor);
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * canvas.scaleFactor);
                //스롯들을 allSlots리스트에 등록
                allSlots.Add(newSlot);
            }
        }

        print(allSlots.Count);
    }
    
    //item maxSize을 체크 하는 함수
    public bool AddItem(Item item)
    {
        //item.maxSize 가 1일때 PlacecEmpty()함수를 실행
        if (item.maxSize == 1)
        {
            //PlaceEmpty함수에 아이템을 넣어 준다
            PlaceEmpty(item);
            return true;
        }
        else
        {
            //allSlots에서 slot을 찾는다
            foreach (GameObject slot in allSlots)
            {
                Slot tmp = slot.GetComponent<Slot>();

                //slot이 비여있지 않다면
                if (!tmp.IsEmpty)
                {
                    //
                    if (tmp.CurrentItem.type == item.type && tmp.IsAvailable)
                    {
                        //인벤토리 아이템에 더하다
                        tmp.AddItem(item);
                        return true;
                    }    
                }
                
            }
            if (emptySlot > 0)
            {
                PlaceEmpty(item);
            }
        }
        return false;
    }


    //비여있는 슬롯을 확인하는 함수
    private bool PlaceEmpty(Item item)
    {
        //emptySlot이 0보다 크다면 
        if (emptySlot > 0)
        {
            foreach (GameObject slot in allSlots)
            {
                //List를 선언하지 않았는데...
                Slot tmp = slot.GetComponent<Slot>();

                if (tmp.IsEmpty)
                {
                    //tmp에 추가를 한다
                    tmp.AddItem(item);
                    //emptySlot에 차감해준다
                    emptySlot--;
                    return true;
                }
            }
        }
        return false;
    }

    public void MoveItem(GameObject clicked)
    {
        Inventory.clicked = clicked;
        print(clicked.name);

        if (!movingSlot.IsEmpty)
        {
            Slot tmp = clicked.GetComponent<Slot>();

            if (tmp.IsEmpty)
            {
                tmp.AddItems(movingSlot.Items);

                movingSlot.Items.Clear();

                Destroy(GameObject.Find("Hover"));
            }
            else if (!tmp.IsEmpty && movingSlot.CurrentItem.type == tmp.CurrentItem.type && tmp.IsAvailable)
            {
                MergeStacks(movingSlot, tmp);
            }
        }
        //from 스롯이 비여있고 캔버스 알파 가 1이면
        else if (from == null && canvasGroup.alpha == 1 && !Input.GetKey(KeyCode.LeftShift))
        {
            //스롯이 비여있지 않고 Hover를 찾을 수 없다면
            if (!clicked.GetComponent<Slot>().IsEmpty && !GameObject.Find("Hover"))
            {

                from = clicked.GetComponent<Slot>();

                from.GetComponent<Image>().color = Color.gray;

                CreateHoverIcon();
            }
        }
        //to 슬롯이 비여있다면
        else if (to == null && !Input.GetKey(KeyCode.LeftShift))
        {
            to = clicked.GetComponent<Slot>();
            Destroy(GameObject.Find("Hover"));
        }
        //to from 슬롯이 비여 않다면
        if (to != null && from != null)
        {
            //tmp 슬롯에 to슬롯을 넣어준다.
            Stack<Item> tmpTo = new Stack<Item>(to.Items);
            //to 슬롯에 from을 넣어 준다
            to.AddItems(from.Items);
            //tmpTo가 0이면
            if (tmpTo.Count == 0)
            {
                from.ClearSlot();
            }
            else
            {
                //from 슬롯에 tmp 슬롯을 넣어 준다
                from.AddItems(tmpTo);
            }

            from.GetComponent<Image>().color = Color.white;
            //to from을 비워 준다
            to = null;
            from = null;
            hoverObject = null;
            Destroy(GameObject.Find("Hover"));
        }
    }

    private void CreateHoverIcon()
    {

        //static hoverObject에 iconPrefab을 넣어준다.
        hoverObject = (GameObject)Instantiate(iconPrefab);
        //clicked 오브젝트의 스프라이트를 hoverObject에 넣어 준다
        hoverObject.GetComponent<Image>().sprite = clicked.GetComponent<Image>().sprite;
        //hoverObject의 이름을 Hover로 바꾸어 줌
        hoverObject.name = "Hover";

        //clicked, hoverObject를 RectTransform 컴포넌트를 등록한다
        RectTransform hoverTransform = hoverObject.GetComponent<RectTransform>();
        RectTransform clickedTransform = clicked.GetComponent<RectTransform>();
        //clickedTransform의 사이즈를 hoverTransform의 사이즈로 넣어줌
        hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.x);
        hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.y);
        //hoverObject의 부모관계와 로컬스케일 설정한다
        hoverObject.transform.SetParent(GameObject.Find("Canvas").transform, true);
        hoverObject.transform.localScale = clicked.gameObject.transform.localScale;

        hoverObject.transform.GetChild(0).GetComponent<Text>().text = movingSlot.Items.Count > 1 ? movingSlot.Items.Count.ToString() : string.Empty;
    }

    private void PutItemBack()
    {
        //from 비여있지 않으면
        if (from != null)
        {
            //Hover 를 지우고 from을 화이트로 바꾸어 준다
            Destroy(GameObject.Find("Hover"));
            from.GetComponent<Image>().color = Color.white;
            from = null;
        }
    }

    public void SetStackInfo(int maxStackCount)
    {
        selectStackSize.SetActive(true);
        splitAmount = 0;
        this.maxStackCount = maxStackCount;
        stackText.text = splitAmount.ToString();
    }

    public void SplitStack()
    {
        selectStackSize.SetActive(false);

        if (splitAmount == maxStackCount)
        {
            MoveItem(clicked);
        }
        else if (splitAmount > 0)
        {
            movingSlot.Items = clicked.GetComponent<Slot>().RemoveItems(splitAmount);

            CreateHoverIcon();
        }
    }

    public void ChangeStackText(int i)
    {
        splitAmount += i;

        if (splitAmount < 0)
        {
            splitAmount = 0;
        }

        if (splitAmount > maxStackCount)
        {
            splitAmount = maxStackCount;
        }

        stackText.text = splitAmount.ToString();
    }

    public void MergeStacks(Slot source, Slot destination)
    {
        int max = destination.CurrentItem.maxSize - destination.Items.Count;

        int count = source.Items.Count < max ? source.Items.Count : max;

        for (int i = 0; i < count; i++)
        {
            destination.AddItem(source.RemoveItem());
        }

        if (source.Items.Count == 0)
        {
            source.ClearSlot();

            Destroy(GameObject.Find("Hover"));
        }
    }

    private IEnumerator FadeOut()
    {
        if (!fadingOut)
        {
            fadingOut = true;
            fadingIn = false;
            StopCoroutine("FadeIn");

            float startAlpha = canvasGroup.alpha;
            float rate = 1f / fadingTime;
            float progress = 0f;

            while (progress < 1)
            {
                canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, progress);

                progress += rate * Time.deltaTime;

                yield return null;
            }

            canvasGroup.alpha = 0;
            fadingOut = false;
        }
    }

    private IEnumerator FadeIn()
    {
        if (!fadingIn)
        {
            fadingOut = false;
            fadingIn = true;
            StopCoroutine("FadeOut");

            float startAlpha = canvasGroup.alpha;
            float rate = 1f / fadingTime;
            float progress = 0f;

            while (progress < 1)
            {
                canvasGroup.alpha = Mathf.Lerp(startAlpha, 1, progress);

                progress += rate * Time.deltaTime;

                yield return null;
            }

            canvasGroup.alpha = 1;
            fadingIn = false;
        }
    }
}
