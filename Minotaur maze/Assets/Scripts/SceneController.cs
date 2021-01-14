using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviourPunCallbacks
{
    public static readonly bool IsMobile = false;

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

    public static bool IsNull(Object obj)
    {
        return obj == null;
    }

    public override void OnLeftRoom()
    {
        LoadScene("PlayerSelection");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
