using UnityEngine;
using System.Collections;

public class CannonCtrl : MonoBehaviour
{
    private Transform tran;

    //PhotonView 컴포넌트 변수
    private PhotonView pv;

    //원격 네트워크 탱크의 포신 회전 각도를 저장할 변수
    private Quaternion currRot = Quaternion.identity;

    public float rotSpeed = 100f;

    // Use this for initialization
    void Awake()
    {
        tran = GetComponent<Transform>();
        pv = GetComponent<PhotonView>();

        //포톤뷰의 Observed 속성을 바꾸어줌
        pv.ObservedComponents[0] = this;
        //Photon View의 동기화 속성을 설정
        pv.synchronization = ViewSynchronization.UnreliableOnChange;

        //초기 회전 값
        currRot = tran.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.isMine)
        {
            float angle = Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * rotSpeed;

            tran.Rotate(angle, 0, 0);
        }
        else
        {
            //현재 회전 각도에서 수신받은 실시간 회전 각도로 부드럽게 회전
            tran.localRotation = Quaternion.Slerp(tran.localRotation, currRot, Time.deltaTime * 3f);
        }
        
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(tran.localRotation);
            print("Cannon stream : " + stream);
        }
        else
        {
            currRot = (Quaternion)stream.ReceiveNext();
            print("Cannon currRot : " + currRot);
        }
    }
}
