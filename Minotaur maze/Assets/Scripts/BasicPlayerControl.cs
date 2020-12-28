using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public abstract class BasicPlayerControl : MonoBehaviour
{
    private Camera _mainCamera;
    protected NavMeshAgent Agent;
    private PhotonView _photonView;
    protected Animator Animator;

    public static FixedJoystick Joystick;
    public static RectTransform WinningPanel;

    protected Vector3 InitPosition;

    private static readonly int Run = Animator.StringToHash("Run");

    private void Start()
    {
        _mainCamera = Camera.main;
        Agent = GetComponent<NavMeshAgent>();
        _photonView = GetComponent<PhotonView>();
        Animator = GetComponent<Animator>();

        InitPosition = Agent.nextPosition;

        InitPlayer();
    }

    private void Update()
    {
        if (!_photonView.IsMine) return;
        UpdateMainCamera();
        JoystickControl();
        UpdatePlayer();
    }

    private void JoystickControl()
    {
        const float deviation = 0.1f;
        const float speed = 20f;
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
        }
        else
        {
            Animator.SetBool(Run, false);
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

    protected static void SetActiveWinningPanel()
    {
        WinningPanel.gameObject.SetActive(true);
    }

    protected abstract void OnTriggerEnter(Collider other);

    protected abstract void WinGame();

    protected abstract void UpdatePlayer();

    protected abstract void InitPlayer();
}
