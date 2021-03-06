﻿using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class MazeGenerator : MonoBehaviour
{
    private readonly List<Vector3> _playersPosition = new List<Vector3>();
    public readonly List<Vector3> MonstersPosition = new List<Vector3>();
    private int _randomSeed;

    public MazeGenerator()
    {
        InitializeMonstersPosition();
    }

    public void Initialize()
    {
        InitializePlayersPosition();
        _randomSeed = Random.Range(1, 100);
    }

    private void InitializePlayersPosition()
    {
        _playersPosition.Add(new Vector3(8, 0, 31));
        _playersPosition.Add(new Vector3(10, 0, 4));
        _playersPosition.Add(new Vector3(3, 0, 16));
        _playersPosition.Add(new Vector3(21, 0, 21));
    }

    private void InitializeMonstersPosition()
    {
        MonstersPosition.Add(new Vector3(24, 0, 5));
        MonstersPosition.Add(new Vector3(5, 0, 0));
        MonstersPosition.Add(new Vector3(7, 0, 23));
        MonstersPosition.Add(new Vector3(27, 0, 32));
        MonstersPosition.Add(new Vector3(15, 0, 10));
        MonstersPosition.Add(new Vector3(17, 0, 20));
    }

    public Hashtable GenerateCustomRoomProperties()
    {
        var properties = new Hashtable
        {
            {CustomPropertyKeys.RandomSeed.ToString(), _randomSeed},
            {CustomPropertyKeys.PlayersPositions.ToString(), _playersPosition.ToArray()},
        };
        return properties;
    }

    public int GetRandomSeed()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(CustomPropertyKeys.RandomSeed.ToString(),
            out var randomSeed))
        {
            return (int) randomSeed;
        }

        return 1;
    }

    public Vector3 GetRandomPlayerPosition()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(CustomPropertyKeys.PlayersPositions.ToString(),
            out var temp))
        {
            var positions = new List<Vector3>((Vector3[]) temp);

            var position = GetRandomPosition(positions);

            positions.Remove(position);

            UpdateCustomPropertyByKey(CustomPropertyKeys.PlayersPositions, positions);

            return position;
        }

        return Vector3.one;
    }

    public Vector3 GetPositionByIndex(int index)
    {
        return MonstersPosition[index];
    }

    private Vector3 GetRandomPosition(IReadOnlyList<Vector3> positions)
    {
        var range = GetRandomRange(positions.Count);

        var position = positions[range];

        return position;
    }

    private int GetRandomRange(int count)
    {
        return Random.Range(0, count);
    }

    private void UpdateCustomPropertyByKey(CustomPropertyKeys key, List<Vector3> positions)
    {
        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable {{key.ToString(), positions.ToArray()}});
    }
}
