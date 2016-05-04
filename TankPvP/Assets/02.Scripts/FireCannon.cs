using UnityEngine;
using System.Collections;

public class FireCannon : MonoBehaviour
{
    //cannon 프리팹을 연결할 변수
    private GameObject cannon = null;
    //포탄 사운드 파일
    private AudioClip fireSfx = null;
    //컴포넌트 할당
    private AudioSource sfx = null;
    //cannon 발사 지점
    public Transform firePos;
    //PhotonView 컴포넌트 변수
    public PhotonView pv = null;

    public void Awake()
    {

        cannon = (GameObject)Resources.Load("Cannon");
        print("Fire");
        fireSfx = Resources.Load<AudioClip>("CannonFire");

        sfx = GetComponent<AudioSource>();

        pv = this.gameObject.GetComponentInParent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (MouseHover.instance.isUIHover)
        {
            return;
        }

        //PhotonView가 자신의 것이고 마우스 오른쪽 클릭시 발사한다.
        else if (Input.GetMouseButtonDown(0))
        {
            print("Fire");

            //자신의 탱크일경우 로컬함수를 호출
            Fire();

            //원격네트워크 플레이어 RPC로 원격으로 Fire함수를 호출
            //pv.RPC("Fire", PhotonTargets.Others, null);
        }
    }

    [PunRPC]
    void Fire()
    {
        
        Instantiate(cannon, firePos.position, firePos.rotation);


        //발사 사운드
        sfx.PlayOneShot(fireSfx, 1f);
    }
}
