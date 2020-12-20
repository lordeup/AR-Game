using System.Collections;
using Photon.Pun;
using UnityEngine;

public class SceneController : MonoBehaviourPunCallbacks
{
    public static void LoadScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }

    public delegate void DelegateWaitMethod();

    public static IEnumerator WaitMethod(DelegateWaitMethod method, float second)
    {
        yield return new WaitForSeconds(second);
        method?.Invoke();
    }
}
