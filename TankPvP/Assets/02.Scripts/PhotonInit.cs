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

    void OnJoineRoom()
    {
        print("Enter Room !");
    }

    public void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }
}
