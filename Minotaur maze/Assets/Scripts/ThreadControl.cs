using UnityEngine;

public class ThreadControl : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var isMageTag = other.CompareTag(GameObjectTag.Mage.ToString());

        var distance = Vector3.Distance(transform.position, other.transform.position);
        if (distance > SceneController.MinDistanceCollider) return;

        if (isMageTag)
        {
            Destroy(gameObject);
        }
    }
}
