using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class FireCtrl : MonoBehaviour
{
    public GameObject bullet;

    public Transform firePos;

    //발사 사운드
    public AudioClip fireSfx;

    //AudioSource 컴포넌트 저장 변수
    private AudioSource source = null;

    //MuzzleFlash 이펙트 오브젝트 등록
    public MeshRenderer muzzleFlash;

    public void Start()
    {
        source = GetComponent<AudioSource>();

        //MuzzleFlash 비활성화
        muzzleFlash.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(firePos.position, firePos.forward * 10f, Color.green);

        if (Input.GetMouseButtonDown(0))
        {
            //Fire();

            //Ray에 맞은 게임오브젝트의 정보 변수
            RaycastHit hit;

            //Raycast 함수로 Ray를 발사해 맞은 게임오브젝트가 있을 때 true를 반환
            if (Physics.Raycast(firePos.position, firePos.forward, out hit, 10f))
            {
                StartCoroutine(ShowMuzzleFlash());
                //Ray에 맞는 게임오브젝트태그 Tag 값을 비교해 몬스터 여부 체크
                if (hit.collider.tag == "MONSTER")
                {
                //    //SendMessage를 이용해 전달한 인자를 배열에 담음
                    object[] _params = new object[2];
                    print(_params.Length);
                    _params[0] = hit.point;                     //Ray에 맞은 정확한 위치값(Vector3)
                    print("hit : " + hit.point);
                    _params[1] = 20;                            //몬스터에 입힐 데미지 값
                    //몬스터에 데미지 입히는 함수 호출
                    hit.collider.gameObject.SendMessage("OnDamage", _params, SendMessageOptions.DontRequireReceiver);
                }

                if (hit.collider.tag == "BARREL")
                {
                    print("Barrel");
                    object[] _params = new object[2];
                    _params[0] = firePos.position;
                    _params[1] = hit.point;
                    hit.collider.gameObject.SendMessage("OnDamage", _params, SendMessageOptions.DontRequireReceiver);
                }
            }
        }
    }

    void Fire()
    {
        CreateBullet();
        //사운드 실행
        //source.PlayOneShot(fireSfx, 0.9f);
        GameMgr.instance.PlaySfx(firePos.position, fireSfx);

        StartCoroutine(ShowMuzzleFlash());
    }

    void CreateBullet()
    {
        Instantiate(bullet, firePos.position, firePos.rotation);
    }

    IEnumerator ShowMuzzleFlash()
    {
        muzzleFlash.enabled = true;

        float scale = Random.Range(1f, 2f);
        muzzleFlash.transform.localScale = Vector3.one * scale;

        Quaternion rot = Quaternion.Euler(0, 0, Random.Range(0, 360));
        muzzleFlash.transform.localRotation = rot;


        yield return new WaitForSeconds(Random.Range(.05f, .03f));

        muzzleFlash.enabled = false;
    }
}
