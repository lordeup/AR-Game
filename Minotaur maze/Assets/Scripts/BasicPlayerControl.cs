using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public abstract class BasicPlayerControl : MonoBehaviour
{
    public static FixedJoystick Joystick;
    public static RectTransform WinningPanel;

    protected NavMeshAgent Agent;
    protected Animator Animator;
    protected Vector3 InitPosition;
    protected SoundManager SoundManager;
    protected bool IsDead;

    private Camera _mainCamera;
    private PhotonView _photonView;

    private static readonly int Run = Animator.StringToHash("Run");
    private static readonly int Jump = Animator.StringToHash("Jump");

    private readonly float _height = SceneController.IsMobile ? 5f : 25f;
    private readonly float _distance = SceneController.IsMobile ? 4f : 10f;
    private const float SmoothSpeed = 0.5f;

    private void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
        SoundManager = GetComponent<SoundManager>();
        InitPosition = Agent.nextPosition;

        _mainCamera = Camera.main;
        _photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (!_photonView.IsMine || IsDead) return;

        JoystickControl();
        UpdatePlayer();
    }

    private void LateUpdate()
    {
        if (_photonView.IsMine)
        {
            UpdateCamera();
        }
    }

    private void UpdateCamera()
    {
        var position = transform.position;
        var mainCameraPosition = _mainCamera.transform.position;

        var worldPosition = Vector3.forward * -_distance + Vector3.up * _height;
        var newPosition = position + worldPosition;

        mainCameraPosition = Vector3.Slerp(mainCameraPosition, newPosition, SmoothSpeed);
        _mainCamera.transform.position = mainCameraPosition;
        _mainCamera.transform.LookAt(position, Vector3.up);
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

    protected abstract void OnCollisionEnter(Collision other);

    protected abstract void UpdatePlayer();
}
