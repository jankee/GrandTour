﻿using UnityEngine;
using System.Collections;

public class BarrelCtrl : MonoBehaviour
{
    //폭발 이펙트
    public GameObject expEffect;

    public GameObject sparkEff;

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

            GameObject spark = (GameObject)Instantiate(sparkEff, colli.transform.position, Quaternion.identity);

            Destroy(colli.gameObject);
            Destroy(spark, spark.GetComponent<ParticleSystem>().duration + 0.5f);

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

        Destroy(this.gameObject, 5.0f);
    }

    void OnDamage(object[] _params)
    {
        //발사 위치
        Vector3 firePos = (Vector3)_params[0];
        //맞은 위치
        Vector3 hitPos = (Vector3)_params[1];
        //입사벡터(Ray의 각도)
        Vector3 incomeVector = hitPos - firePos;
        print("IncomeVector : " + incomeVector);
        //입사벡터를 정규화(Normalized)벡터로 변경
        incomeVector = incomeVector.normalized;
        print("IncomeVector Normalized : " + incomeVector);
        //Ray의 hit좌표에 입사벡터의 각도로 힘을 생성
        GetComponent<Rigidbody>().AddForceAtPosition(incomeVector * 1000f, hitPos);


        GameObject spark = (GameObject)Instantiate(sparkEff, hitPos, Quaternion.identity);

        if (++hitCount > 3)
        {
            ExpBarrel();
        }
    }
}
