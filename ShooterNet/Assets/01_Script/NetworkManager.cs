using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
    //접속 IP
    private const string ip = "127.0.0.1";
    //접속 포트
    private const int port = 30000;
    //NAT 기능의 사용 여부
    private bool _useNat = false;
    //플레이어 프리펩
    public GameObject player;

    void OnGUI()
    {
        //현재 사용자의 네트워크에 접속여부 판단
        if (Network.peerType == NetworkPeerType.Disconnected)
        {
            //게임 서버 생성
            if (GUI.Button(new Rect(20, 20, 200, 25), "Start Server"))
            {
                //게임 서버 생성 : InitializeServer(접속자수, 포트번호, NAT사용여부)
                Network.InitializeServer(20, port, _useNat);
            }
            //게임에 접속하는 버튼
            if (GUI.Button(new Rect(20, 50, 200, 25), "Connect to Server"))
            {
                //게임 서버 접속 : Connect(접속IP, 접속포트번호)
                Network.Connect(ip, port);
            }

        }
        else
        {
            //서버일 때 메시지 출력
            if (Network.peerType == NetworkPeerType.Server)
            {
                GUI.Label(new Rect(20, 20, 200, 25), "Initialization Server...");
                GUI.Label(new Rect(20, 50, 200, 25), "Client Count = " + Network.connections.Length.ToString());
            }

            //클라이언트로 접속했을때 메시지 출력
            if (Network.peerType == NetworkPeerType.Client)
            {
                GUI.Label(new Rect(20, 20, 200, 25), "Connect to Server");
            }
        }
    }

    public void OnServerInitialized()
    {
        CreatePlayer();
    }

    public void OnConnectedToServer()
    {
        CreatePlayer();
    }

    void CreatePlayer()
    {
        Vector3 pos = new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));

        Network.Instantiate(player, pos, Quaternion.identity, 0);
    }
}
