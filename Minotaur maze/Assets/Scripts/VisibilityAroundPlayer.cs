using UnityEngine;

public class VisibilityAroundPlayer : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        var cubeVisibility = other.GetComponent<CubeVisibility>();
        var component = other.GetComponent<Renderer>();

        if (SceneController.IsNull(cubeVisibility) || SceneController.IsNull(component)) return;

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

        if (SceneController.IsNull(cubeVisibility) || SceneController.IsNull(component)) return;

        var isFogTag = other.CompareTag(GameObjectTag.Fog.ToString());

        if (isFogTag && cubeVisibility.IsVisible)
        {
            component.enabled = false;
        }
    }
}
