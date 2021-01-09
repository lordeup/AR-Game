using UnityEngine;

public class VisibilityAroundPlayer : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        var component = other.GetComponent<Renderer>();
        var isFogTag = other.CompareTag(GameObjectTag.Fog.ToString());

        if (isFogTag && !SceneController.IsNull(component))
        {
            component.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var component = other.GetComponent<Renderer>();
        var isFogTag = other.CompareTag(GameObjectTag.Fog.ToString());

        if (isFogTag && !SceneController.IsNull(component))
        {
            component.enabled = false;
        }
    }
}
