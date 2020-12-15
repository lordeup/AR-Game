using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public class PlayerControls : MonoBehaviour
{
    private Camera _mainCamera;
    private NavMeshAgent _agent;
    private PhotonView _photonView;
    private Animator _animator;

    public static FixedJoystick Joystick;
    private const float speed = 10f;
    private Vector3 _initPosition;

    private void Start()
    {
        _mainCamera = Camera.main;
        _agent = GetComponent<NavMeshAgent>();
        _photonView = GetComponent<PhotonView>();
        _animator = GetComponent<Animator>();

        _initPosition = _agent.transform.localPosition;
    }

    private void Update()
    {
        UpdateMainCamera();
        if (!_photonView.IsMine) return;

        // if (Math.Abs(Joystick.Horizontal) > 0.1f)
        // {
        //     var direction = Vector3.forward * Joystick.Vertical + Vector3.right * Joystick.Horizontal;
        //     var pos = direction * speed;
        //     // var direction = Vector3.forward * Joystick.Vertical + Vector3.right * Joystick.Horizontal;
        //     // var v = direction * (speed * Time.fixedDeltaTime);
        //
        //     _animator.Play("walk");
        //     _agent.SetDestination(pos);
        // }


        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(_mainCamera.ScreenPointToRay(Input.mousePosition), out var raycastHit))
            {
                _animator.Play("walk");
                _agent.SetDestination(raycastHit.point);
            }
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

    private IEnumerator RespawnPlayerAfterDelay()
    {
        yield return new WaitForSeconds(1.0f);
        RespawnPlayer();
    }

    private void RespawnPlayer()
    {
        var pos = new Vector3(0, 0, 0);
        var transform1 = _agent.transform;
        _agent.destination = pos;
        transform1.localPosition = _initPosition;
        transform1.rotation = Quaternion.identity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
            if (gameObject.CompareTag("PlayerWithWeapon"))
            {
                _animator.Play("attack01");
            }
            else if (gameObject.CompareTag("PlayerWithThread"))
            {
                StartCoroutine(RespawnPlayerAfterDelay());
            }
        }
    }
}
