using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform prefabPlayerWithWeapon;
    [SerializeField] private Transform prefabPlayerWithThread;
    [SerializeField] private Transform prefabMonster;

    private MazeSpawner _mazeSpawner;
    private NavMeshSurface _navMeshSurface;

    private const int MaxNumberPlayers = 3;

    private const int _randomSeed = 10;
    // _randomSeed = Random.Range(1, 100);

    private void Start()
    {
        _mazeSpawner = GetComponent<MazeSpawner>();
        _navMeshSurface = GetComponent<NavMeshSurface>();

        _mazeSpawner.RandomSeed = _randomSeed;
        SetActive();
        InitializationPlayers();
        InitializationMonsters();

        gameObject.AddComponent<NavMeshRebaker>();
    }

    private void Update()
    {
    }

    private void SetActive()
    {
        _mazeSpawner.enabled = true;
        _navMeshSurface.enabled = true;
    }

    private void InitializationPlayers()
    {
        var playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        if (playerCount == MaxNumberPlayers) return;
        var player = prefabPlayerWithWeapon;
        if (playerCount > 1)
        {
            player = prefabPlayerWithThread;
        }

        var pos = new Vector3(0, 0, 0);
        PhotonNetwork.Instantiate(player.name, pos, Quaternion.identity);
    }

    private void InitializationMonsters()
    {
        for (var i = 0; i < 5; ++i)
        {
            var pos = new Vector3(Random.Range(5f, 40f), 0);
            PhotonNetwork.Instantiate(prefabMonster.name, pos, Quaternion.identity);
        }
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
