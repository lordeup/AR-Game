using Photon.Pun;

public class SceneController : MonoBehaviourPunCallbacks
{
    public static void LoadScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }
}
