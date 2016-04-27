using UnityEngine;
using System.Collections;

public class TurretCtrl : MonoBehaviour
{
    private Transform tran;
    //광선이 지면에 맞은 위치
    private RaycastHit hit;
    //터렛의 회전 속도
    public float turretSpeed = 5f;

    //포톤뷰 컴포넌트 변수
    private PhotonView pv = null;
    //원격 네트워크 탱크의 터렛 회전값을 저장할 변수
    private Quaternion currRot = Quaternion.identity;


    // Use this for initialization
    void Awake()
    {
        tran = GetComponent<Transform>();
        pv = GetComponent<PhotonView>();

        //Photon View의 Observed 속성을 이 스크립트로 지정
        pv.ObservedComponents[0] = this;
        //Photon View의 동기화 속성을 설정
        pv.synchronization = ViewSynchronization.UnreliableOnChange;

        currRot = tran.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.isMine)
        {
            //메인카메라에서 마우스 커서의 위치로 캐스팅되는 Ray를 생성
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //print(ray);
            Debug.DrawLine(ray.origin, ray.direction * 100f, Color.red);

            //Physics.Raycast(광선, out 충돌정보, 거리, 레이어 마스크)
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 8))
            {
                //Ray에 맞는 위치를 로컬좌표로 변환
                Vector3 relative = tran.InverseTransformPoint(hit.point);
                //역탄젠트 함수인 Atan2로 두 점간의 각도를 계산
                float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
                //rotSpeed 변수에 지정된 속도로 회전
                tran.Rotate(0, angle * Time.deltaTime * turretSpeed, 0);
            }
        }
        //원격 네트워크 플레이어의 탱크일 경우
        else
        {
            //회전각도에서 수신받은 실시간 회전각도 부드럽게 회전
            tran.localRotation = Quaternion.Slerp(tran.localRotation, currRot, Time.deltaTime * 3f);
        }
    }

    //송수신 콜백 함수
    void OnPhotonSerializView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(tran.localRotation);
        }
        else
        {
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
