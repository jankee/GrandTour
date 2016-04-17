using UnityEngine;
using System.Collections;

public class TankMove : MonoBehaviour 
{
    //탱크의 이동 및 회전 속도를 나타내는 변수
    public float moveSpeed = 20f;
    public float rotSpeed = 50f;

    //참조할 컴포넌트를 할당 할 변수
    private Rigidbody rigiBody;
    private Transform trans;

    //키보드 입력값 변수
    private float h, v;

	// Use this for initialization
	void Start () 
    {
	    //컴포넌트 할당
        rigiBody = GetComponent<Rigidbody>();
        trans = GetComponent<Transform>();

        //RigiBody의 무게중심을 낮게 설정
        rigiBody.centerOfMass = new Vector3(0f, -0.5f, 0f);
	}
	
	// Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        //회전과 이동 처리
        trans.Rotate(Vector3.up * rotSpeed * h * Time.deltaTime);
        trans.Translate(Vector3.forward * moveSpeed * v * Time.deltaTime);
    }
}
