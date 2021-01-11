using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public class GameController : MonoBehaviour
{
    [SerializeField] private Transform prefabWarriorPlayer;
    [SerializeField] private Transform prefabMagePlayer;
    [SerializeField] private Transform prefabMonster;
    [SerializeField] private RectTransform winningPanel;
    [SerializeField] private FixedJoystick joystick;

    private MazeSpawner _mazeSpawner;
    private NavMeshSurface _navMeshSurface;
    private ThreadCountControl _threadCountControl;
    private MazeGenerator _mazeGenerator;
    private PlayerType _playerType;

    private void Start()
    {
        _mazeGenerator = gameObject.AddComponent<MazeGenerator>();
        _mazeSpawner = GetComponent<MazeSpawner>();
        _navMeshSurface = GetComponent<NavMeshSurface>();

        _playerType = PlayerSelectionManager.PlayerType;

        if (_playerType == PlayerType.Spectator)
        {
            _mazeSpawner.IsVisibleFog = false;
        }

        _mazeSpawner.RandomSeed = _mazeGenerator.GetRandomSeed();
        _mazeSpawner.enabled = true;
        _navMeshSurface.enabled = true;

        InitializationPlayers();
        if (PhotonNetwork.IsMasterClient)
        {
            InitializationMonsters();
        }

        gameObject.AddComponent<NavMeshRebaker>();
        BasicPlayerControl.WinningPanel = winningPanel;
    }

    private void InitializationPlayers()
    {
        var player = GetPlayer(_playerType);

        if (SceneController.IsNull(player)) return;

        var randomPlayerPosition = _mazeGenerator.GetRandomPlayerPosition();

        PhotonNetwork.Instantiate(player.name, randomPlayerPosition, Quaternion.identity);

        joystick.gameObject.SetActive(true);
        BasicPlayerControl.Joystick = joystick;

        if (_playerType == PlayerType.Mage)
        {
            _threadCountControl = GetComponent<ThreadCountControl>();
            _threadCountControl.enabled = true;
            MagePlayerControl.ThreadCountControls = _threadCountControl;
            MagePlayerControl.FloorsWithLines = _mazeSpawner.FloorsWithLines;
        }
    }

    private Transform GetPlayer(PlayerType playerType)
    {
        switch (playerType)
        {
            case PlayerType.Mage:
                return prefabMagePlayer;
            case PlayerType.Warrior:
                return prefabWarriorPlayer;
            case PlayerType.Spectator:
                return null;
            default:
                throw new ArgumentOutOfRangeException(nameof(playerType), playerType, null);
        }
    }

    private void InitializationMonsters()
    {
        for (var i = 0; i < 9; ++i)
        {
            var position = _mazeGenerator.GetPositionByIndex(i);
            PhotonNetwork.Instantiate(prefabMonster.name, position, Quaternion.identity);
        }
    }
}
