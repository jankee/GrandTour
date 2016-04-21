using UnityEngine;
using System.Collections;

public class TurretCtrl : MonoBehaviour
{
    private Transform tran;
    //광선이 지면에 맞은 위치
    private RaycastHit hit;
    //터렛의 회전 속도
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

        //Physics.Raycast(광선, out 충돌정보, 거리, 레이어 마스크)
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1<<8))
        {
            //Ray에 맞는 위치를 로컬좌표로 변환
            Vector3 relative = tran.InverseTransformPoint(hit.point);
            //역탄젠트 함수인 Atan2로 두 점간의 각도를 계산
            float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
            //rotSpeed 변수에 지정된 속도로 회전
            tran.Rotate(0, angle * Time.deltaTime * turretSpeed, 0);
        }
    }
}
