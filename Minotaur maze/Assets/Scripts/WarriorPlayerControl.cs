using UnityEngine;

public class WarriorPlayerControl : BasicPlayerControl
{
    private static readonly int Property = Animator.StringToHash("Attack 01");
    private static readonly int Jump = Animator.StringToHash("Jump");

    public WarriorPlayerControl() : base(GameObjectTag.Warrior)
    {
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(GameObjectTag.Monster.ToString()))
        {
            Animator.SetTrigger(Property);
        }

        if (other.gameObject.CompareTag(GameObjectTag.Wall.ToString()))
        {
            WinGame();
        }
    }

    protected override void WinGame()
    {
        Animator.SetTrigger(Jump);
        StartCoroutine(SceneController.WaitMethod(SetActiveWinningPanel, 2.5f));
    }
}
