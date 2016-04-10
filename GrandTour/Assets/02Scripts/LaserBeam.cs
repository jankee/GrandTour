using UnityEngine;
using System.Collections;

public class LaserBeam : MonoBehaviour 
{
    private Transform tr;
    private LineRenderer line;

    //광선에 충돌한 게임 오브젝트의 정보를 받아올 변수
    private RaycastHit hit;

	// Use this for initialization
	void Start () 
    {
	    //컴포넌트 할당
        tr = GetComponent<Transform>();
        line = GetComponent<LineRenderer>();

        //로컬좌표를 기준으로 변경
        line.useWorldSpace = false;
        //초기에 비활성화
        line.enabled = false;
        //라인의 시작폭과 종료폭 설정
        line.SetWidth(.3f, .01f);
	}
	
	// Update is called once per frame
	void Update () 
    {
        Ray ray = new Ray(tr.position + (Vector3.up * .02f), tr.forward);

        Debug.DrawRay(ray.origin, ray.direction * 100, Color.blue);

        if (Input.GetMouseButtonDown(0))
        {
            //Line Renderer의 첫 번째 점의 위치 설정
            line.SetPosition(0, tr.InverseTransformPoint(ray.origin));

            //어떤 물체에 광선이 맞았을 때의 위치를 Line Renderer의 끝점으로 설정
            if (Physics.Raycast(ray, out hit, 100f))
            {
                line.SetPosition(1, tr.InverseTransformPoint(hit.point));
            }
            else
	        {
                line.SetPosition(1, tr.InverseTransformPoint(ray.GetPoint(100f)));
	        }

            StartCoroutine(ShowLaserBeam());
        }
	}

    IEnumerator ShowLaserBeam()
    {
        line.enabled = true;
        yield return new WaitForSeconds(Random.Range(.01f, .2f));
        line.enabled = false;
    }
}
