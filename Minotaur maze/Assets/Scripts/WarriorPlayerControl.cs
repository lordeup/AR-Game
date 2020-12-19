using UnityEngine;

public class WarriorPlayerControl : BasicPlayerControl
{
    private static readonly int Property = Animator.StringToHash("Attack 01");

    public WarriorPlayerControl() : base(GameObjectTag.Warrior)
    {
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(GameObjectTag.Monster.ToString()))
        {
            Animator.SetTrigger(Property);
        }
    }
}
