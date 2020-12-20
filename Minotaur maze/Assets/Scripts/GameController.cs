using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform prefabWarriorPlayer;
    [SerializeField] private Transform prefabMagePlayer;
    [SerializeField] private Transform prefabMonster;
    [SerializeField] private FixedJoystick joystick;

    private MazeSpawner _mazeSpawner;
    private NavMeshSurface _navMeshSurface;
    private ThreadCountControl _threadCountControl;

    private const int MaxNumberPlayers = 3;

    private void Start()
    {
        _mazeSpawner = GetComponent<MazeSpawner>();
        _navMeshSurface = GetComponent<NavMeshSurface>();

        _mazeSpawner.RandomSeed = Helper.RandomSeed;
        SetActive();
        InitializationPlayers();
        InitializationMonsters();

        gameObject.AddComponent<NavMeshRebaker>();
    }

    private void Update()
    {
        if (_mazeSpawner.isGenerate)
        {
            var walls = GameObject.FindGameObjectsWithTag("Wall");
            _mazeSpawner.isGenerate = false;
        }
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
        var player = prefabWarriorPlayer;
        if (playerCount > 1)
        {
            player = prefabMagePlayer;
        }

        PhotonNetwork.Instantiate(player.name, Helper.GetRandomPlayerPosition(), Quaternion.identity);
        Debug.Log("_playersPosition.Count " + Helper._playersPosition.Count);

        BasicPlayerControl.Joystick = joystick;

        if (player.CompareTag(GameObjectTag.Mage.ToString()))
        {
            _threadCountControl = GetComponent<ThreadCountControl>();
            _threadCountControl.enabled = true;
            BasicPlayerControl.ThreadCountControl = _threadCountControl;
        }
    }

    private void InitializationMonsters()
    {
        for (var i = 0; i <= 10; ++i)
        {
            Instantiate(prefabMonster, Helper.GetRandomMonsterPosition(), Quaternion.identity);
        }

        Debug.Log("_monstersPosition.Count " + Helper._monstersPosition.Count);
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
