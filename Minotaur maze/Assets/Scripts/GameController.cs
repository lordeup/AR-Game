﻿using System;
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

    private void Start()
    {
        _mazeGenerator = gameObject.AddComponent<MazeGenerator>();
        _mazeSpawner = GetComponent<MazeSpawner>();
        _navMeshSurface = GetComponent<NavMeshSurface>();

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
        var playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

        if (playerCount == PhotonNetwork.CurrentRoom.MaxPlayers) return;

        var player = prefabWarriorPlayer;

        if (playerCount > 1)
        {
            player = prefabMagePlayer;
        }

        var randomPlayerPosition = _mazeGenerator.GetRandomPlayerPosition();

        PhotonNetwork.Instantiate(player.name, new Vector3(30, 0, 0), Quaternion.identity);

        BasicPlayerControl.Joystick = joystick;

        if (player.CompareTag(GameObjectTag.Mage.ToString()))
        {
            _threadCountControl = GetComponent<ThreadCountControl>();
            _threadCountControl.enabled = true;
            MagePlayerControl.ThreadCountControls = _threadCountControl;
            MagePlayerControl.FloorsWithLines = _mazeSpawner.FloorsWithLines;
        }
    }

    private void InitializationMonsters()
    {
        for (var i = 0; i <= 10; ++i)
        {
            var position = _mazeGenerator.GetPositionByIndex(i);
            PhotonNetwork.Instantiate(prefabMonster.name, position, Quaternion.identity);
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
