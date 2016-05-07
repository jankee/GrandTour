using UnityEngine;
using System.Collections;

public class RoadCreator : MonoBehaviour
{
    //기본이 되는 로드 오브젝트
    public GameObject roadObj;

    //도로의 갯수
    public int roadNum;

    //배열로 묶을 변수
    private GameObject[] roadObjs = null;

    public void RoadBuilder()
    {
        roadObjs = new GameObject[roadNum];

        for (int i = 0; i < roadNum; i++)
        {
            roadObjs[i] = (GameObject)Instantiate(roadObj, new Vector3(0, 0, 0), Quaternion.identity);            
        }

        for (int i = 0; i < roadObjs.Length; i++)
        {
            if (i == 0)
            {
                roadObjs[i].transform.position = Vector3.zero;
            }
            else
            {
                roadObjs[i].transform.position = roadObjs[i - 1].transform.FindChild("EndPoint").position;
            }
            
        }

        print("Create Road");
    }

}
