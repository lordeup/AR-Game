using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MagePlayerControl : BasicPlayerControl
{
    private static readonly int Die = Animator.StringToHash("Die");

    public static ThreadCountControl ThreadCountControls;

    public static Dictionary<Transform, List<LineRenderer>> FloorsWithLines =
        new Dictionary<Transform, List<LineRenderer>>();

    private readonly List<Transform> _distancePassed = new List<Transform>();
    private KeyValuePair<Transform, List<LineRenderer>> _previousFloorAndLines;
    private KeyValuePair<Transform, List<LineRenderer>> _currentFloorAndLines;

    protected override void OnTriggerEnter(Collider other)
    {
        var isMonsterTag = other.CompareTag(GameObjectTag.Monster.ToString());
        var isWallTag = other.CompareTag(GameObjectTag.Wall.ToString());
        var isThreadTag = other.CompareTag(GameObjectTag.Thread.ToString());

        if (isMonsterTag && !IsDead)
        {
            IsDead = true;
            Animator.SetTrigger(Die);
            SoundManager.PlayDeathSound();
            StartCoroutine(SceneController.WaitMethod(RespawnPlayer, 2.5f));
        }

        if (isThreadTag && !SceneController.IsNull(ThreadCountControls))
        {
            SoundManager.PlayThreadSound();
            ThreadCountControls.AddCount(5);
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
        IsDead = false;
    }

    protected override void UpdatePlayer()
    {
        if (SceneController.IsNull(ThreadCountControls) ||
            !ThreadCountControls.GetActiveThreadMode() ||
            ThreadCountControls.GetCount() == 0)
        {
            return;
        }

        _currentFloorAndLines = GetCurrentFloorAndLines();

        if (!IsEmptyKeyValuePair(_previousFloorAndLines)
            && !IsEmptyKeyValuePair(_currentFloorAndLines)
            && !_currentFloorAndLines.Equals(_previousFloorAndLines))
        {
            var floor = _previousFloorAndLines.Key;
            _distancePassed.Add(floor);
            ThreadCountControls.UpdateCountOnDistancePassed();
            ActivateLines(_previousFloorAndLines.Value);
        }

        _previousFloorAndLines = _currentFloorAndLines;
    }

    private static bool IsEmptyKeyValuePair(KeyValuePair<Transform, List<LineRenderer>> pair)
    {
        return pair.Equals(default(KeyValuePair<Transform, List<LineRenderer>>));
    }

    private static void ActivateLines(IEnumerable<LineRenderer> lines)
    {
        foreach (var line in lines)
        {
            line.enabled = true;
        }
    }

    private KeyValuePair<Transform, List<LineRenderer>> GetCurrentFloorAndLines()
    {
        var currentPosition = Agent.transform.position;

        return FloorsWithLines.FirstOrDefault(item =>
        {
            var floor = item.Key;

            var boxCollider = floor.gameObject.GetComponent<BoxCollider>();

            return IsCurrentFloorPosition(boxCollider.bounds, currentPosition) && !IsExists(_distancePassed, floor);
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

    private static bool IsExists<T>(IEnumerable<T> list, T value)
    {
        return list.Any(item => item.Equals(value));
    }
}
