using System.Collections.Generic;
using System.Linq;
using Packages.Rider.Editor.UnitTesting;
using UnityEngine;

public class MagePlayerControl : BasicPlayerControl
{
    private static readonly int Die = Animator.StringToHash("Die");
    private static readonly int Jump = Animator.StringToHash("Jump");

    private List<Transform> _distancePassed = new List<Transform>();
    private Transform _currentFloor;

    protected override void OnTriggerEnter(Collider other)
    {
        var isMonsterTag = other.CompareTag(GameObjectTag.Monster.ToString());
        var isWallTag = other.CompareTag(GameObjectTag.Wall.ToString());
        var isThreadTag = other.CompareTag(GameObjectTag.Thread.ToString());

        if (isMonsterTag)
        {
            Animator.SetTrigger(Die);
            StartCoroutine(SceneController.WaitMethod(RespawnPlayer, 2.5f));
        }

        if (isThreadTag)
        {
            // TODO если игрок с нитью подберет нить в мультиплеере возникает ошибка для игрока с мечом
            // NullReferenceException: Object reference not set to an instance of an object
            // ThreadCount.AddCount(5);
            // Debug.Log("!@@@@@@@@@@ " + ThreadCount.GetCount());
            ThreadCount.AddCount(5);
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

    protected override void UpdatePlayer()
    {
        _currentFloor = GetCurrentFloor();

        if (_currentFloor != null)
        {
            Debug.Log(_currentFloor.position);
            _distancePassed.Add(_currentFloor);
        }
    }

    private Transform GetCurrentFloor()
    {
        var currentPosition = Agent.transform.position;
        
        return FloorList.FirstOrDefault(item =>
        {
            var boxCollider = item.gameObject.GetComponent<BoxCollider>();

            var bounds = boxCollider.bounds;

            return IsCurrentFloorPosition(bounds, currentPosition) && !IsPassed(item);
        });
    }

    private bool IsCurrentFloorPosition(Bounds bounds, Vector3 currentPosition)
    {
        return currentPosition.sqrMagnitude > bounds.min.sqrMagnitude
               && currentPosition.sqrMagnitude < bounds.max.sqrMagnitude;
    }

    private bool IsPassed(Transform floor)
    {
        return _distancePassed.Any(item => item == floor);
    }
}
