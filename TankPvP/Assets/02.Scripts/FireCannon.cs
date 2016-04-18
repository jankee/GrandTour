using UnityEngine;
using System.Collections;

public class FireCannon : MonoBehaviour
{
    //cannon 프리팹을 연결할 변수
    public GameObject cannon = null;
    //cannon 발사 지점
    public Transform firePos;

    public void Awake()
    {
        cannon = (GameObject)Resources.Load("Cannon");
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
    }
}
