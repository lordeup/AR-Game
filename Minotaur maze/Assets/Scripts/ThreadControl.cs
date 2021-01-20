using UnityEngine;

public class ThreadControl : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        var isMageTag = other.gameObject.CompareTag(GameObjectTag.Mage.ToString());

        if (isMageTag)
        {
            Destroy(gameObject);
        }
    }
}
