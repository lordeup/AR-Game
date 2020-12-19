using UnityEngine;

public class ThreadControl : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(GameObjectTag.Mage.ToString()))
        {
            Destroy(gameObject);
        }
    }
}
