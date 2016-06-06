using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public float speed;

    public Inventory inventory;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        //HandleMovement 함수 실행
        HandleMovement();
    }

    //움직임 조정 함수
    private void HandleMovement()
    {
        //translarion 지역 변수에 speed * Time.deltaTime을 곱하여 넣어준다
        float translation = speed * Time.deltaTime;
        //Translate함수를 실행하여 Input을 받아 움직임을 만든다.
        this.transform.Translate(new Vector3(Input.GetAxis("Horizontal") * translation,
            0, Input.GetAxis("Vertical") * translation));

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ITEM")
        {
            //인벤토리의 AddItem()함수를 실행한다
            inventory.AddItem(other.GetComponent<Item>());
        }
    }

}
