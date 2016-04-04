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
        Debug.DrawRay(firePos.position, firePos.forward, Color.green);

        if (Input.GetMouseButtonDown(0))
        {
            Fire();
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
