using UnityEngine;

public class WarriorPlayerControl : BasicPlayerControl
{
    private static readonly int Property = Animator.StringToHash("Attack 01");
    private static readonly int Jump = Animator.StringToHash("Jump");

    protected override void OnTriggerEnter(Collider other)
    {
        var isMonsterTag = other.CompareTag(GameObjectTag.Monster.ToString());
        var isWallTag = other.CompareTag(GameObjectTag.Wall.ToString());

        if (isMonsterTag)
        {
            Animator.SetTrigger(Property);
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

    protected override void WinGame()
    {
        Animator.SetTrigger(Jump);
        StartCoroutine(SceneController.WaitMethod(SetActiveWinningPanel, 2.5f));
    }
}
