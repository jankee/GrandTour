using UnityEngine;
using System.Collections;

public class BarrelCtrl : MonoBehaviour
{
    //폭발 이펙트
    public GameObject expEffect;

    //드럼통의 Transform 좌표
    private Transform tr;

    //텍스쳐배열
    public Texture[] textures;

    //총알 맞은 누적 변수
    private int hitCount = 0;

    public void Start()
    {
        
        tr = GetComponent<Transform>();

        int idx = Random.Range(0, textures.Length);
        //자식컴포넌트 중 MeshRenderer를 찾아 텍스쳐를 바꿔준다.
        GetComponentInChildren<MeshRenderer>().material.mainTexture = textures[idx];
        print("find material" + idx);
    }

    public void OnCollisionEnter(Collision colli)
    {
        print("HI");
        if (colli.collider.tag == "BULLET")
        {
            Destroy(colli.gameObject);

            //총알 맞은 횟수 증가
            if (++hitCount > 3)
            {
                print("Explosion");
                ExpBarrel();

            }
        }
    }

    public void ExpBarrel()
    {
        //폭발 이펙트 효과
        Instantiate(expEffect, tr.transform.position, Quaternion.identity);

        //지정한 원점 중심으로 10.0f 반경 내에 들어와 있는 Collider 객체 추축
        Collider[] colls = Physics.OverlapSphere(tr.position, 10f);

        //추축한 Collider 객체에 폭발력 전달
        foreach (Collider coll in colls)
        {
            Rigidbody rbody = coll.GetComponent<Rigidbody>();

            if (rbody != null)
            {
                rbody.mass = 1.0f;
                rbody.AddExplosionForce(1000f, tr.transform.position, 10f, 300f);
            }
        }

        Destroy(gameObject, 5.0f);
    }
}
