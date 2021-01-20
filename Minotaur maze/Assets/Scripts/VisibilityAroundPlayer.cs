using Photon.Pun;
using UnityEngine;

public class VisibilityAroundPlayer : MonoBehaviour
{
    private PhotonView _photonView;

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void OnTriggerExit(Collider other)
    {
        var cubeVisibility = other.GetComponent<CubeVisibility>();
        var component = other.GetComponent<Renderer>();

        if (SceneController.IsNull(_photonView) ||
            SceneController.IsNull(cubeVisibility) ||
            SceneController.IsNull(component) ||
            !_photonView.IsMine)
        {
            return;
        }

        var isFogTag = other.CompareTag(GameObjectTag.Fog.ToString());

        if (isFogTag && cubeVisibility.IsVisible)
        {
            component.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var cubeVisibility = other.GetComponent<CubeVisibility>();
        var component = other.GetComponent<Renderer>();

        if (SceneController.IsNull(_photonView) ||
            SceneController.IsNull(cubeVisibility) ||
            SceneController.IsNull(component) ||
            !_photonView.IsMine)
        {
            return;
        }

        var isFogTag = other.CompareTag(GameObjectTag.Fog.ToString());

        if (isFogTag && cubeVisibility.IsVisible)
        {
            component.enabled = false;
        }
    }
}
