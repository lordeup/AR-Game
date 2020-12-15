using UnityEngine;

public class WarriorPlayerControls : BasicPlayerControls
{
    public WarriorPlayerControls() : base(GameObjectTag.Warrior)
    {
        
    }
    
    protected override void OnTriggerEnter(Collider other)
    {
        if (gameObject.CompareTag(_gameObjectTag.ToString()) && other.gameObject.CompareTag(GameObjectTag.Monster.ToString()))
        {
            _animator.Play("attack01");
        }
    }
}
