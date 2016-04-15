using UnityEngine;
using System.Collections;
//SmoothFollow 스크립트를 사용하기 위해 네임 스페이스 선언
using UnityStandardAssets.Utility;

public class PlayerCtrl : MonoBehaviour
{
    //컴포턴트 변수 할당
    private Transform tr;
    private NetworkView _networkView;

    public void Awake()
    {
        //컴포넌트 할당
        tr = GetComponent<Transform>();
        _networkView = GetComponent<NetworkView>();

        //NetworkView가 자신의 것인지 확인한다
        if (_networkView.isMine)
        {
            Camera.main.GetComponent<SmoothFollow>().target = tr;
        }
    }
}
