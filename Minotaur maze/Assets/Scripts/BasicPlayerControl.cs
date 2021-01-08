using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public abstract class BasicPlayerControl : MonoBehaviour
{
    public static FixedJoystick Joystick;
    public static RectTransform WinningPanel;
    public static List<GameObject> MazeElements = new List<GameObject>();

    protected NavMeshAgent Agent;
    protected Animator Animator;
    protected Vector3 InitPosition;
    protected SoundManager SoundManager;
    protected bool IsDead;

    private Camera _mainCamera;
    private PhotonView _photonView;

    private static readonly int Run = Animator.StringToHash("Run");
    private static readonly int Jump = Animator.StringToHash("Jump");

    private void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
        SoundManager = GetComponent<SoundManager>();
        InitPosition = Agent.nextPosition;

        // _mainCamera = Camera.main;
        _photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (!_photonView.IsMine || IsDead) return;

        // UpdateMainCamera();
        JoystickControl();
        UpdatePlayer();
    }

    private void JoystickControl()
    {
        const float deviation = 0.1f;
        const float speed = 30f;
        var joystickHorizontal = Joystick.Horizontal;
        var joystickVertical = Joystick.Vertical;

        if (Math.Abs(joystickHorizontal) > deviation || Math.Abs(joystickVertical) > deviation)
        {
            var nextPosition = Agent.nextPosition;
            var delta = Time.fixedDeltaTime * speed;
            var destination = new Vector3(nextPosition.x + joystickHorizontal * delta, nextPosition.y,
                nextPosition.z + joystickVertical * delta);

            Agent.SetDestination(destination);
            Animator.SetBool(Run, true);
            SoundManager.PlayWalkingSound();
        }
        else
        {
            Animator.SetBool(Run, false);
            SoundManager.Stop();
        }
    }

    private void UpdateMainCamera()
    {
        var direction = (Vector3.up * 2 + Vector3.back) * 2;
        if (Physics.Linecast(transform.position, transform.position + direction, out var hit))
        {
            _mainCamera.transform.position = hit.point;
        }
        else
        {
            _mainCamera.transform.position = transform.position + direction;
        }

        _mainCamera.transform.LookAt(transform.position);
    }

    protected void WinGame()
    {
        if (!_photonView.IsMine) return;

        SoundManager.PlayWinSound();
        Animator.SetTrigger(Jump);
        StartCoroutine(SceneController.WaitMethod(SetActiveWinningPanel, 2.5f));
    }

    private static void SetActiveWinningPanel()
    {
        WinningPanel.gameObject.SetActive(true);
    }

    protected abstract void OnTriggerEnter(Collider other);

    protected abstract void UpdatePlayer();
}
