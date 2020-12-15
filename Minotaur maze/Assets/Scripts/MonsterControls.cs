using UnityEngine;
using System.Collections;

public class MonsterControls : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private IEnumerator DestroyMonsterAfterDelay()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy();
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(GameObjectTag.Warrior.ToString()))
        {
            _animator.Play("Die");
            StartCoroutine(DestroyMonsterAfterDelay());
        }
        else if (other.gameObject.CompareTag(GameObjectTag.Mage.ToString()))
        {
            _animator.Play("Attack 01");
        }
    }
}
