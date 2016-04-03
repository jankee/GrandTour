using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour
{
    //추적할 타겟 오브젝트
    public Transform targetTr;
    //카메라와 일정 거리
    public float dist = 10f;
    //카메라의 높이
    public float height = 3f;
    //부드러운 추적을 위한 값
    public float dampTrace = 20f;

    //카메라 자신의 Transform변수
    private Transform camTr;



	// Use this for initialization
	void Start ()
    {
        camTr = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void LateUpdate ()
    {
        camTr.position = Vector3.Lerp(camTr.position, targetTr.position - (targetTr.forward * dist) +
            (Vector3.up * height), Time.deltaTime * dampTrace);
        camTr.LookAt(targetTr.position);
	}
}
