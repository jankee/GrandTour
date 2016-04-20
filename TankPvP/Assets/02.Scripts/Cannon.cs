using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour
{
    //포탄의 속도
    public float speed = 6000f;

    //폭발효과 프리팹
    public GameObject expEffect;

    private CapsuleCollider _collider;

    private Rigidbody _rigiBody;

    GameObject cannon = null;

    // Use this for initialization
    void Start()
    {
        //컴포넌트 연결
        _collider = GetComponent<CapsuleCollider>();

        _rigiBody = GetComponent<Rigidbody>();

        GetComponent<Rigidbody>().AddForce(transform.forward * speed);

        StartCoroutine(this.ExplosionCannon(3f));
    }

    public void OnTriggerEnter()
    {
        print("Fire!");
        StartCoroutine(this.ExplosionCannon(0f));
    }

    IEnumerator ExplosionCannon(float tm)
    {
        yield return new WaitForSeconds(tm);
        //다른 오브젝트랑 충돌이 없도록 Collider를 비활성화
        _collider.enabled = false;
        //물리엔지 영향을 안받음
        _rigiBody.isKinematic = true;

        GameObject obj = (GameObject)Instantiate(expEffect, transform.position, Quaternion.identity);


        Destroy(obj, 1f);

        Destroy(this.gameObject, 1f);
    }
}
