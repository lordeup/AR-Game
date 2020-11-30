using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomController : MonoBehaviourPunCallbacks
{
    private SceneController _sceneController;
    public TextMeshProUGUI playersInformation;
    
    private void Start()
    {
        _sceneController = gameObject.AddComponent<SceneController>();
    }
    
    public override void OnLeftRoom()
    {
        _sceneController.LoadScene("MainMenu");
    }
    
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        Debug.Log("log off from room");
    }
    
    public override void OnPlayerEnteredRoom(Player player)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", player.NickName);
        Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount.ToString());
        
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
        }
        
        UpdateViewOfPlayers();
    }
    
    public override void OnPlayerLeftRoom(Player player)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", player.NickName); 


        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
        }

        UpdateViewOfPlayers();
    }

    private void UpdateViewOfPlayers()
    {
        playersInformation.text = "";
        
        foreach (var player in PhotonNetwork.PlayerList)
        {
            playersInformation.text += player.NickName + '\n';
        }
    }
}