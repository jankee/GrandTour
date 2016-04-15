using UnityEngine;
using System.Collections;

public class MoveCtrl : MonoBehaviour
{
    //컴포넌트 할당 할 변수
    private Transform tr;
    private CharacterController controller;

    //키보드 입력값
    private float h = 0f;
    private float v = 0f;

    //이동속도, 회전속도 변수
    public float movSpeed = 5.0f;
    public float rotSpeed = 50.0f;

    //이동할 방향 벡터 변수
    private Vector3 movDir = Vector3.zero;


	// Use this for initialization
	void Start ()
    {
        //변수 할당
        tr = GetComponent<Transform>();
        controller = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        //마우스 좌우 이동 값으로 회전
        tr.Rotate(Vector3.up * rotSpeed * Input.GetAxis("Mouse X") * Time.deltaTime);
        //이동방향을 벡터의 덧셈연산을 이용해 미리 계산
        movDir = (tr.forward * v) + (tr.right * h);
        //중력값 설정
        movDir.y -= 20f * Time.deltaTime;
        //플레이어를 이동
        controller.Move(movDir * movSpeed * Time.deltaTime);
	}
}
