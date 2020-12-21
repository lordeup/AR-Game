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
        // Agent.destination = Vector3.zero;
        Debug.Log(" 1) RespawnPlayer " + InitPosition);
        var transform1 = Agent.transform;
        transform1.localPosition = InitPosition;
        transform1.rotation = Quaternion.identity;
        // Agent.nextPosition = InitPosition;
        Debug.Log(" 2) Agent.nextPosition " + Agent.nextPosition);
        Agent.ResetPath();
    }

    protected override void WinGame()
    {
        Animator.SetTrigger(Jump);
        StartCoroutine(SceneController.WaitMethod(SetActiveWinningPanel, 2.5f));
    }
}
