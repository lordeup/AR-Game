using System;
using Photon.Pun;
using UnityEngine;
public class LobbyManager : MonoBehaviourPunCallbacks
{
    private SceneController _sceneController;
    
    private void Start()
    {
        _sceneController = gameObject.AddComponent<SceneController>();
    }

    public void CreateRoom() 
    {
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = 3 });
        Debug.Log("Room created");
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("joined room");
        
        _sceneController.LoadScene("Room");
    }
}