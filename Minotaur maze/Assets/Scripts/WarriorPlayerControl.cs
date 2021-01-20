using UnityEngine;

public class WarriorPlayerControl : BasicPlayerControl
{
    private static readonly int Attack = Animator.StringToHash("Attack 01");

    protected override void OnCollisionEnter(Collision other)
    {
        var isMonsterTag = other.gameObject.CompareTag(GameObjectTag.Monster.ToString());
        var isWallTag = other.gameObject.CompareTag(GameObjectTag.Wall.ToString());

        if (isMonsterTag)
        {
            Animator.SetTrigger(Attack);
            SoundManager.PlayFightSound();
        }

        if (isWallTag)
        {
            var meshCollider = other.gameObject.GetComponent<MeshCollider>();

            if (!SceneController.IsNull(meshCollider))
            {
                WinGame();
            }
        }
    }

    protected override void UpdatePlayer()
    {
    }
}
