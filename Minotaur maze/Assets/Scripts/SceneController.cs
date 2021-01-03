using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviourPunCallbacks
{
    public static PhotonView PhotonView;

    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public delegate void DelegateWaitMethod();

    public static IEnumerator WaitMethod(DelegateWaitMethod method, float second)
    {
        yield return new WaitForSeconds(second);
        method?.Invoke();
    }

    public override void OnLeftRoom()
    {
        if (PhotonView != null && PhotonView.IsMine)
        {
            LoadScene("MainMenu");
        }
    }
}
