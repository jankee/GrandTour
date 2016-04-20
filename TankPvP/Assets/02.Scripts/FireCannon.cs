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

    public void Awake()
    {
        cannon = (GameObject)Resources.Load("Cannon");
        fireSfx = Resources.Load<AudioClip>("CannonFire");

        sfx = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    void Fire()
    {
        Instantiate(cannon, firePos.position, firePos.rotation);

        //발사 사운드
        sfx.PlayOneShot(fireSfx, 1f);
    }
}
