using UnityEngine;

public class MonsterControl : MonoBehaviour
{
    private Animator _animator;
    private static readonly int Die = Animator.StringToHash("Die");
    private static readonly int Property = Animator.StringToHash("Attack 01");

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        var isWarriorTag = other.CompareTag(GameObjectTag.Warrior.ToString());
        var isMageTag = other.CompareTag(GameObjectTag.Mage.ToString());

        if (isWarriorTag)
        {
            _animator.SetTrigger(Die);
            StartCoroutine(SceneController.WaitMethod(Destroy, 0.5f));
        }
        else if (isMageTag)
        {
            _animator.SetTrigger(Property);
        }
    }
}
