using UnityEngine;
using System.Collections;

public class TurretCtrl : MonoBehaviour
{
    private Transform tran;
    //광선이 지면에 맞은 위치
    private RaycastHit hit;

    public float turretSpeed = 5f;

    // Use this for initialization
    void Start()
    {
        tran = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //메인카메라에서 마우스 커서의 위치로 캐스팅되는 Ray를 생성
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //print(ray);
        Debug.DrawLine(ray.origin, ray.direction * 100f, Color.red);

    }
}
