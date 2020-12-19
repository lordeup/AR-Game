using UnityEngine;
using System.Collections;

public class MonsterControl : MonoBehaviour
{
    private Animator _animator;
    private static readonly int Die = Animator.StringToHash("Die");
    private static readonly int Property = Animator.StringToHash("Attack 01");

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private IEnumerator DestroyMonsterAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(GameObjectTag.Warrior.ToString()))
        {
            _animator.SetTrigger(Die);
            StartCoroutine(DestroyMonsterAfterDelay());
        }
        else if (other.gameObject.CompareTag(GameObjectTag.Mage.ToString()))
        {
            _animator.SetTrigger(Property);
        }
    }
}
