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

    //PhotonView 컴포넌트를 할당할 변수
    private PhotonView pv = null;
    //메인 카메라가 추적할 CamPivot게임 오브젝트
    public Transform camPivot;

    //위치 정보를 송수신할 때 사용할 변수 선언 및 초기값 설정
    private Vector3 currPos = Vector3.zero;
    private Quaternion currRot = Quaternion.identity;

	// Use this for initialization
	void Awake () 
    {
	    //컴포넌트 할당
        rigiBody = GetComponent<Rigidbody>();
        trans = GetComponent<Transform>();

        //RigiBody의 무게중심을 낮게 설정
        rigiBody.centerOfMass = new Vector3(0f, -0.5f, 0f);

        //PhotonView 컴포넌트 할당
        pv = GetComponent<PhotonView>();

        //데이터 전송 타입을 설정
        pv.synchronization = ViewSynchronization.UnreliableOnChange;

        //PhotonView Observed Components 속성에 TankMove 스크립트를 연결
        pv.ObservedComponents[0] = this;

        //PhotonView가 자신의 탱크일 경우
        if (pv.isMine)
        {
            //메인 카메라에 추가된 SmoothFollow 스크립트에 추적 대사을 연결
            Camera.main.GetComponent<SmoothFollow>().target = camPivot;
            //메인 카메라에 추가돈 SmoothFollow 스크립트에 추적 대상을 연결
            rigiBody.centerOfMass = new Vector3(0f, -0.5f, 0f);
        }
        else
        {
            rigiBody.isKinematic = true;
        }

        currPos = trans.position;
        currRot = trans.rotation;
	}

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(trans.position);
            stream.SendNext(trans.rotation);
        }
        else
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }
	
	// Update is called once per frame
    void Update()
    {
        if (!pv.isMine)
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");

            trans.Rotate(Vector3.up * rotSpeed * h * Time.deltaTime);
            trans.Translate(Vector3.forward * moveSpeed * v * Time.deltaTime);
        }
        else
        {
            //원격 플레이어의 탱크를 수신받은 위치까지 부드럽게 이동시킴
            trans.position = Vector3.Lerp(trans.position, currPos, Time.deltaTime);
        }

        

        //회전과 이동 처리
        
    }
}
