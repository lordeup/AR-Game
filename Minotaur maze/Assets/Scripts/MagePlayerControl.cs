using UnityEngine;

public class MagePlayerControl : BasicPlayerControl
{
    private static readonly int Die = Animator.StringToHash("Die");
    private static readonly int Jump = Animator.StringToHash("Jump");

    public MagePlayerControl() : base(GameObjectTag.Mage)
    {
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(GameObjectTag.Monster.ToString()))
        {
            Animator.SetTrigger(Die);
            StartCoroutine(SceneController.WaitMethod(RespawnPlayer, 2.5f));
        }

        if (other.gameObject.CompareTag(GameObjectTag.Thread.ToString()))
        {
            ThreadCountControl.AddCount(5);
            Debug.Log("!@@@@@@@@@@ " + ThreadCountControl.GetCount());
        }

        if (other.gameObject.CompareTag(GameObjectTag.Wall.ToString()))
        {
            WinGame();
        }
    }

    private void RespawnPlayer()
    {
        Agent.Warp(InitPosition);
        Agent.transform.rotation = Quaternion.identity;
    }

    protected override void WinGame()
    {
        Animator.SetTrigger(Jump);
        StartCoroutine(SceneController.WaitMethod(SetActiveWinningPanel, 2.5f));
    }
}
