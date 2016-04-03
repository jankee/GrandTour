using UnityEngine;
using System.Collections;

public class DelegateTest : MonoBehaviour 
{
    //델리케이트에 연결할 함수의 원형 정의
    delegate void CalNumDelegate(int num);

    //델리게이트 변수 정의
    CalNumDelegate calNum;

	// Use this for initialization
	void Start () 
    {
        //CalNum 델리게이트 변수에 OnePlusNum 함수 연결
        calNum = OnePlusNum;

        //함수 호출
        calNum(4);

        calNum = PowerNum;

        calNum(5);

	}
    
	void OnePlusNum(int num)
    {
        int result = num + 1;
        print("One Plus = " + result);
    }

    void PowerNum(int num)
    {
        int result = num * num;
        print("Power = " + result);
    }
}
