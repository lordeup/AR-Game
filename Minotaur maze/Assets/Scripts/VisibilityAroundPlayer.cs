using UnityEngine;

public class VisibilityAroundPlayer : MonoBehaviour
{
    private Collider[] _hitColliders;

    // Вешаем на объект Rigidbody
    // Вешаем на объект любой подходящий коллайдер(нам сойдёт куб), задаём его размеры так, чтобы он охватывал чуть больше чем ширину коридора лабиринта
    // Ставим на коллайдере галку isTrigger
    // Вешаем на объект этот скрипт.
    // По итогу скрипт будет отображать все объекты, которые попадают в наш коллайдер-триггер
    private void OnTriggerExit(Collider other)
    {
        var component = other.GetComponent<Renderer>();

        if (!SceneController.IsNull(component))
        {
            // component.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var component = other.GetComponent<Renderer>();

        if (!SceneController.IsNull(component))
        {
            // component.enabled = true;
        }

        ExplosionDamage(other.gameObject.transform.localPosition, 2);
    }

    private void ExplosionDamage(Vector3 center, float radius)
    {
        _hitColliders = Physics.OverlapSphere(center, radius);

        // foreach (var hitCollider in hitColliders)
        // {
        //     var isMageTag = hitCollider.gameObject.CompareTag(GameObjectTag.Mage.ToString());
        //     var isWarriorTag = hitCollider.gameObject.CompareTag(GameObjectTag.Warrior.ToString());
        //
        //     if (isMageTag || isWarriorTag) continue;
        //
        //     hitCollider.gameObject.SetActive(false);
        // }
    }
}
