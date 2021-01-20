using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MagePlayerControl : BasicPlayerControl
{
    private static readonly int Die = Animator.StringToHash("Die");

    public static ThreadCountControl ThreadCountControls;

    public static Dictionary<Transform, Tuple<Transform, List<LineRenderer>>> FloorsWithLines =
        new Dictionary<Transform, Tuple<Transform, List<LineRenderer>>>();

    private readonly List<Transform> _distancePassed = new List<Transform>();
    private KeyValuePair<Transform, Tuple<Transform, List<LineRenderer>>> _previousFloorAndLines;

    protected override void OnCollisionEnter(Collision other)
    {
        var isMonsterTag = other.gameObject.CompareTag(GameObjectTag.Monster.ToString());
        var isWallTag = other.gameObject.CompareTag(GameObjectTag.Wall.ToString());
        var isThreadTag = other.gameObject.CompareTag(GameObjectTag.Thread.ToString());

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
            var meshCollider = other.gameObject.GetComponent<MeshCollider>();

            if (!SceneController.IsNull(meshCollider))
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

        var currentFloorAndLines = GetCurrentFloorAndLines();
        var previousFloor = _previousFloorAndLines.Key;

        if (!IsEmptyKeyValuePair(_previousFloorAndLines) &&
            !IsExists(_distancePassed, previousFloor) &&
            !currentFloorAndLines.Equals(_previousFloorAndLines))
        {
            _distancePassed.Add(previousFloor);
            ThreadCountControls.UpdateCountOnDistancePassed();
            ActivateLines(_previousFloorAndLines.Value);
        }

        _previousFloorAndLines = currentFloorAndLines;
    }

    private static bool IsEmptyKeyValuePair(KeyValuePair<Transform, Tuple<Transform, List<LineRenderer>>> pair)
    {
        return pair.Equals(default(KeyValuePair<Transform, Tuple<Transform, List<LineRenderer>>>));
    }

    private static void ActivateLines(Tuple<Transform, List<LineRenderer>> tuple)
    {
        var (item1, item2) = tuple;

        if (!SceneController.IsNull(item1))
        {
            var cubeVisibility = item1.GetComponent<CubeVisibility>();
            var component = item1.GetComponent<Renderer>();

            if (SceneController.IsNull(cubeVisibility) || SceneController.IsNull(component)) return;

            cubeVisibility.IsVisible = false;
            component.enabled = false;
        }

        foreach (var line in item2)
        {
            line.enabled = true;
        }
    }

    private KeyValuePair<Transform, Tuple<Transform, List<LineRenderer>>> GetCurrentFloorAndLines()
    {
        var currentPosition = Agent.transform.position;

        return FloorsWithLines.FirstOrDefault(item =>
        {
            var floor = item.Key;

            var boxCollider = floor.gameObject.GetComponent<BoxCollider>();

            return IsCurrentFloorPosition(boxCollider.bounds, currentPosition);
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
