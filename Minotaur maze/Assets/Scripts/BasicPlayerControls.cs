﻿using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public abstract class BasicPlayerControls : MonoBehaviour
{
    protected Camera _mainCamera;
    protected NavMeshAgent _agent;
    protected PhotonView _photonView;
    protected Animator _animator;

    protected static FixedJoystick Joystick;
    protected const float speed = 10f;
    protected Vector3 _initPosition;

    protected readonly GameObjectTag _gameObjectTag;

    protected BasicPlayerControls(GameObjectTag gameObjectTag)
    {
        _gameObjectTag = gameObjectTag;
    }

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

    protected abstract void OnTriggerEnter(Collider other);
}
