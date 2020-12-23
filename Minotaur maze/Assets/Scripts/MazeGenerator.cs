using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class MazeGenerator : MonoBehaviour
{
    private readonly List<Vector3> _playersPosition = new List<Vector3>();
    private readonly List<Vector3> _monstersPosition = new List<Vector3>();
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
        _playersPosition.Add(Vector3.one);
        _playersPosition.Add(new Vector3(10, 0, 4));
        _playersPosition.Add(new Vector3(11, 0, 15));
        _playersPosition.Add(new Vector3(15, 0, 7));
        _playersPosition.Add(new Vector3(18, 0, 0));
        _playersPosition.Add(new Vector3(21, 0, 21));
        _playersPosition.Add(new Vector3(25, 0, 30));
    }

    private void InitializeMonstersPosition()
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
        return _monstersPosition[index];
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
