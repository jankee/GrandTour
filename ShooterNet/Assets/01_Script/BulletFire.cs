using UnityEngine;
using System.Collections;

public class BulletFire : MonoBehaviour 
{
    //총알 생명 시간
    private float lifeTime = 5f;

	// Use this for initialization
	void Start () 
    {
	    //rigibody의 속도를 Forward 방향으로 설정
        GetComponent<Rigidbody>().velocity = transform.forward * 10f;

        //일정 시간 후 제거
        Destroy(gameObject, lifeTime);
	}
}
