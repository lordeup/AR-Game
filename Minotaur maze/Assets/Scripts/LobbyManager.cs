using Photon.Pun;
using UnityEngine;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private SceneController _sceneController;
    private MazeGenerator _mazeGenerator;

    private void Start()
    {
        _mazeGenerator = gameObject.AddComponent<MazeGenerator>();
        _sceneController = gameObject.AddComponent<SceneController>();
    }

    public void CreateRoom()
    {
        _mazeGenerator.Initialize();
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions {MaxPlayers = 3, CustomRoomProperties = _mazeGenerator.GenerateCustomRoomProperties()});
        Debug.Log("Room created");
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("joined room");

        SceneController.LoadScene("Room");
    }
}
