using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Helper
{
    // public static Helper ins = null;
    public static List<Vector3> _playersPosition = new List<Vector3>();
    public static List<Vector3> _monstersPosition = new List<Vector3>();
    public static int RandomSeed;

    public static void Initialize()
    {
        InitializePlayersPosition();
        InitializeMonstersPosition();
        RandomSeed = Random.Range(1, 100);
    }

    private static void InitializePlayersPosition()
    {
        _playersPosition.Add(Vector3.one);
        _playersPosition.Add(new Vector3(10, 0, 4));
        _playersPosition.Add(new Vector3(11, 0, 15));
        _playersPosition.Add(new Vector3(15, 0, 7));
        _playersPosition.Add(new Vector3(18, 0, 0));
        _playersPosition.Add(new Vector3(21, 0, 21));
        _playersPosition.Add(new Vector3(25, 0, 30));
    }

    private static void InitializeMonstersPosition()
    {
        _monstersPosition.Add(new Vector3(16, 0, 2));
        _monstersPosition.Add(new Vector3(3, 0, 0));
        _monstersPosition.Add(new Vector3(7, 0, 23));
        _monstersPosition.Add(new Vector3(32, 0, 9));
        _monstersPosition.Add(new Vector3(34, 0, 11));
        _monstersPosition.Add(new Vector3(13, 0, 12));
        _monstersPosition.Add(new Vector3(19, 0, 10));
        _monstersPosition.Add(new Vector3(9, 0, 19));
        _monstersPosition.Add(new Vector3(17, 0, 20));
        _monstersPosition.Add(new Vector3(31, 0, 27));
        _monstersPosition.Add(new Vector3(11, 0, 29));
        _monstersPosition.Add(new Vector3(36, 0, 3));
    }

    private static Vector3 GetRandomPosition([NotNull] List<Vector3> vector3S)
    {
        if (vector3S == null) throw new ArgumentNullException(nameof(vector3S));
        var range = Random.Range(0, vector3S.Count);
        var position = vector3S[range];
        vector3S.Remove(position);

        return position;
    }

    public static Vector3 GetRandomPlayerPosition()
    {
        return GetRandomPosition(_playersPosition);
    }

    public static Vector3 GetRandomMonsterPosition()
    {
        return GetRandomPosition(_monstersPosition);
    }
}
