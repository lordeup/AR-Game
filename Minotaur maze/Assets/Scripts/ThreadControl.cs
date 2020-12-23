using UnityEngine;

public class ThreadControl : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var isMageTag = other.CompareTag(GameObjectTag.Mage.ToString());

        if (isMageTag)
        {
            Destroy(gameObject);
        }
    }
}
