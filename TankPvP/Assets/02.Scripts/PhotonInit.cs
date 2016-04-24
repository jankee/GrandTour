using UnityEngine;
using System.Collections;

public class PhotonInit : MonoBehaviour
{
    public string version = "v1.0";

    public void Awake()
    {
        PhotonNetwork.ConnectUsingSettings(version);
    }

    void OnJoinedLobby()
    {
        print("Entered Lobby !");

        PhotonNetwork.JoinRandomRoom();
    }

    void OnPhotonRandomJoinFailed()
    {
        print("No Room!");

        PhotonNetwork.CreateRoom("MyRoom");
    }

    void OnJoinedRoom()
    {
        print("Enter Room !");

        CreateTank();
    }

    public void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }

    void CreateTank()
    {
        float pos = Random.Range(-100f, 100f);

        PhotonNetwork.Instantiate("Tank", new Vector3(pos, 20f, pos), Quaternion.identity, 0);
    }
}
