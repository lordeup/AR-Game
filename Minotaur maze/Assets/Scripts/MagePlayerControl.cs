using System.Collections;
using UnityEngine;

public class MagePlayerControl : BasicPlayerControl
{
    private static readonly int Die = Animator.StringToHash("Die");

    public MagePlayerControl() : base(GameObjectTag.Mage)
    {
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(GameObjectTag.Monster.ToString()))
        {
            Animator.SetTrigger(Die);
            StartCoroutine(RespawnPlayerAfterDelay());
        }

        if (other.gameObject.CompareTag(GameObjectTag.Thread.ToString()))
        {
            ThreadCountControl.AddCount(5);
            Debug.Log("!@@@@@@@@@@ " + ThreadCountControl.GetCount());
        }
    }

    private IEnumerator RespawnPlayerAfterDelay()
    {
        yield return new WaitForSeconds(2.5f);
        RespawnPlayer();
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
}
