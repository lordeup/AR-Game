using UnityEngine;

public class VisibilityAroundPlayer : MonoBehaviour
{
    private void Update()
    {
        // var colliders = Physics.OverlapSphere(transform.position, 2);
        //
        // foreach (var item in colliders)
        // {
        //     var isFogTag = item.CompareTag(GameObjectTag.Fog.ToString());
        //
        //     if (!isFogTag) continue;
        //
        //     var component = item.gameObject.GetComponent<Renderer>();
        //
        //     if (!SceneController.IsNull(component))
        //     {
        //         component.enabled = true;
        //     }
        // }
    }

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
