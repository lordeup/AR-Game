using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class MagePlayerControl : BasicPlayerControl
{
    private static readonly int Die = Animator.StringToHash("Die");
    private static readonly int Jump = Animator.StringToHash("Jump");
    
    public static Toggle ThreadModeToggle;
    public static List<LineRenderer> LineRenderers;

    private List<Transform> _distancePassed = new List<Transform>();
    private Transform _currentFloor;
    private bool _isActiveThreadMode;

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
        if (!_isActiveThreadMode || ThreadCount.GetCount() == 0) return;
        
        _currentFloor = GetCurrentFloor();

        if (_currentFloor != null)
        {
            Debug.Log(LineRenderers);
            Debug.Log(_currentFloor.position);
            _distancePassed.Add(_currentFloor);
            ThreadCount.UpdateCountOnDistancePassed();
        }
    }

    protected override void InitPlayer()
    {
        ThreadModeToggle.onValueChanged.AddListener(UpdateThreadMode);
    }

    private void UpdateThreadMode(bool value)
    {
        Debug.Log(value);
        _isActiveThreadMode = value;
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

    private static bool IsCurrentFloorPosition(Bounds bounds, Vector3 currentPosition)
    {
        var isWithinX = IsWithin(currentPosition.x, bounds.min.x, bounds.max.x);
        var isWithinZ = IsWithin(currentPosition.z, bounds.min.z, bounds.max.z);

        return isWithinX && isWithinZ;
    }

    private static bool IsWithin<T>(T value, T minimum, T maximum) where T : IComparable<T>
    {
        if (value.CompareTo(minimum) < 0)
        {
            return false;
        }

        return value.CompareTo(maximum) <= 0;
    }

    private bool IsPassed(Object floor)
    {
        return _distancePassed.Any(item => item == floor);
    }
}
