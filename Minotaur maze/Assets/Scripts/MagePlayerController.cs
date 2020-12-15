using System.Collections;
using UnityEngine;

public class MagePlayerController : BasicPlayerControls
{
    public MagePlayerController() : base(GameObjectTag.Mage)
    {
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (gameObject.CompareTag(_gameObjectTag.ToString()) && other.gameObject.CompareTag(GameObjectTag.Monster.ToString()))
        {
            StartCoroutine(RespawnPlayerAfterDelay());
        }
    }
    
    private IEnumerator RespawnPlayerAfterDelay()
    {
        yield return new WaitForSeconds(1.0f);
        RespawnPlayer();
    }

    private void RespawnPlayer()
    {
        var pos = new Vector3(0, 0, 0);
        var transform1 = _agent.transform;
        _agent.destination = pos;
        transform1.localPosition = _initPosition;
        transform1.rotation = Quaternion.identity;
    }
}
