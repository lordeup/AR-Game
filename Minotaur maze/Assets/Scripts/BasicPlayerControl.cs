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
    public static ThreadCountControl ThreadCountControl;

    private const float Speed = 20f;
    protected Vector3 InitPosition;

    protected readonly GameObjectTag ObjectTag;
    private static readonly int Run = Animator.StringToHash("Run");

    protected BasicPlayerControl(GameObjectTag gameObjectTag)
    {
        ObjectTag = gameObjectTag;
    }

    private void Start()
    {
        _mainCamera = Camera.main;
        Agent = GetComponent<NavMeshAgent>();
        _photonView = GetComponent<PhotonView>();
        Animator = GetComponent<Animator>();

        InitPosition = Agent.nextPosition;
        Debug.Log(" Start " + InitPosition);
    }

    private void Update()
    {
        // UpdateMainCamera();
        if (!_photonView.IsMine) return;

        if (Math.Abs(Joystick.Horizontal) > 0.1f || Math.Abs(Joystick.Vertical) > 0.1f)
        {
            var nextPosition = Agent.nextPosition;
            var delta = Time.fixedDeltaTime * Speed;
            var destination = new Vector3(nextPosition.x + Joystick.Horizontal * delta, nextPosition.y,
                nextPosition.z + Joystick.Vertical * delta);

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

    protected abstract void OnTriggerEnter(Collider other);
}
