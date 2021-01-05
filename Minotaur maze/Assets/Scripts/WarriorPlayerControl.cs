using UnityEngine;

public class WarriorPlayerControl : BasicPlayerControl
{
    private static readonly int Attack = Animator.StringToHash("Attack 01");

    protected override void OnTriggerEnter(Collider other)
    {
        var isMonsterTag = other.CompareTag(GameObjectTag.Monster.ToString());
        var isWallTag = other.CompareTag(GameObjectTag.Wall.ToString());

        var distance = Vector3.Distance(transform.position, other.transform.position);
        if (distance > SceneController.MinDistanceCollider) return;

        if (isMonsterTag)
        {
            Animator.SetTrigger(Attack);
            SoundManager.PlayFightSound();
        }

        if (isWallTag)
        {
            var meshCollider = other.GetComponent<MeshCollider>();
            if (meshCollider.isTrigger)
            {
                WinGame();
            }
        }
    }

    protected override void UpdatePlayer()
    {
    }
}
