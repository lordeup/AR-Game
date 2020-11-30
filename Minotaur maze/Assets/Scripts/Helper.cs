using Photon.Pun;

public class SceneController : MonoBehaviourPunCallbacks
{
    public void LoadScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }
}


