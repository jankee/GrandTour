using UnityEngine;
using System.Collections;
//SmoothFollow 스크립트를 사용하기 위해 네임 스페이스 선언
using UnityStandardAssets.Utility;

public class PlayerCtrl : MonoBehaviour
{
    public enum AnimState
    {
        //애니메이션 배열의 Index에 맞게 0부터 시작
        idle = 0,
        runForward,
        runBackward,
        runRight,
        runLeft,
    }

    //애니메이션 상태를 저장하는 변수
    public AnimState animState = AnimState.idle;
    //사용할 애니메이션 클립 배열
    public AnimationClip[] animClips;

    //CharactorController 컴포넌트를 할당할 변수
    private CharacterController controller;
    //하위의 Animation 컴포너트를 할당할 변수
    private Animation anim;

    //컴포턴트 변수 할당
    private Transform tr;
    private NetworkView _networkView;

    //위치 정보를 송수신 할때 사용할 변수 선언 및 초깃값 설정
    private Vector3 currPos = Vector3.zero;

    private Quaternion currRot = Quaternion.identity;

    //Bullet 프리팹 할당
    public GameObject bullet;
    //총알 발사 위치
    public Transform firePos;

    //사망 여부를 나타내는 변수
    private bool isDie = false;
    //플레이어 생명치
    private int hp = 100;
    //부활 시간
    private float respawnTime = 3.0f;



    public void Awake()
    {
        //컴포넌트 할당
        tr = GetComponent<Transform>();
        _networkView = GetComponent<NetworkView>();

        //자신의 스크립트를 Observed 속성에 연결
        //_networkView.observed = this;

        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animation>();

        //NetworkView가 자신의 것인지 확인한다
        if (_networkView.isMine)
        {
            Camera.main.GetComponent<SmoothFollow>().target = tr;
        }
    }

    void Update()
    {
        if (_networkView.isMine)
        {
            //로컬 플레이어 로직

            if (Input.GetMouseButtonDown(0))
            {
                if (isDie)
                {
                    return;
                }

                Fire();

                //자신을 제외한 나머지 원격 사용자에게 Fire 함수를 원격 호출
                _networkView.RPC("Fire", RPCMode.Others);
            }

            //CharactorController의 속도벡터를 로컬벡터로 변환
            Vector3 localVelocity = tr.InverseTransformDirection(controller.velocity);
            //전진후진 방향의 가속도
            Vector3 forwardDir = new Vector3(0f, 0f, localVelocity.z);
            //좌우방향의 가속도
            Vector3 rightDir = new Vector3(localVelocity.x, 0f, 0f);

            //전후진, 좌우 방향에 따라 애니메이션 상태 설정
            if (forwardDir.z >= 0.1f)
            {
                animState = AnimState.runForward;
            }
            else if (forwardDir.z <= -0.1f)
            {
                animState = AnimState.runBackward;
            }
            else if (rightDir.x >= 0.1f)
            {
                animState = AnimState.runRight;
            }
            else if (rightDir.x <= -0.1f)
            {
                animState = AnimState.runLeft;
            }
            else
            {
                animState = AnimState.idle;
            }
            //애니메이션 실행
            anim.CrossFade(animClips[(int)animState].name, 0.2f);
        }
        else
        {
            if (Vector3.Distance(tr.position, currPos) >= 2f)
            {
                tr.position = currPos;
                tr.rotation = currRot;
            }
            else
            {
                //전송받아온 변경된 위치로 부드럽게 이동
                tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 10.0f);

                //전송받아온 변경된 각도로 부드럽게 회전
                tr.rotation = Quaternion.Slerp(tr.rotation, currRot, Time.deltaTime * 10.0f);
            }
            //애니메이션 실행
            anim.CrossFade(animClips[(int)animState].name, 0.2f);
        }
    }

    //RPC함수 지정을 위해 반드시 [RPC] 어트리뷰트를 명시
    [RPC]
    void Fire()
    {
        GameObject.Instantiate(bullet, firePos.position, firePos.rotation);
    }

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        //로컬 플레이어의 위치 및 회전 정보 송신
        if (stream.isWriting)
        {
            Vector3 pos = tr.position;
            Quaternion rot = tr.rotation;
            int _animState = (int)animState;
            print("_animState" + _animState);

            //데이터 전송
            stream.Serialize(ref pos);
            stream.Serialize(ref rot);
            stream.Serialize(ref _animState);
        }
        //원격 플레이어의 위치 및 회전 정보 수신
        else
        {
            Vector3 revPos = Vector3.zero;
            Quaternion revRot = Quaternion.identity;
            int _animState = 0;

            //데이터 수신
            stream.Serialize(ref revPos);
            stream.Serialize(ref revRot);
            stream.Serialize(ref _animState);

            currPos = revPos;
            currRot = revRot;
            animState = (AnimState)_animState;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BULLET")
        {
            //생명 감소
            hp -= 20;

            Destroy(other.gameObject);

            if (hp <= 0)
            {
                StartCoroutine(this.RespawnPlayer(respawnTime));
            }
        }
    }

    IEnumerator RespawnPlayer(float waitTime)
    {
        isDie = true;

        //플레이어의 Mesh Renderer를 비활성화하는 코루틴 함수 호출
        StartCoroutine(this.PlayerVisible(false, 0.0f));

        //Respawn 시간까지 기다림
        yield return new WaitForSeconds(waitTime);

        //시간이 지난 후 플레이어의 위치를 무작위로 산출
        tr.position = new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f));

        //생명치를 초기값으로 재 설정
        hp = 100;

        //플레이어를 컨트롤할 수 있게 변수 설정
        isDie = false;
        //플레이어의 mesh Renderer 활성화
    }

    IEnumerator PlayerVisible(bool visible, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        //플레이어의 Skinned Mesh Renderer 활성 비활성화
        GetComponentInChildren<SkinnedMeshRenderer>().enabled = visible;

        //플레이어의 Weapon의 Mesh Renderer 활성 비활성화
        GetComponentInChildren<MeshRenderer>().enabled = visible;

        if (_networkView.isMine)
        {
            GetComponent<MoveCtrl>().enabled = visible;
            controller.enabled = visible;
        }
    }


}
