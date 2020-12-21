using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
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
}
