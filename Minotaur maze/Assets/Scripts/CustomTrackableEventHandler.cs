﻿using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Vuforia;

public class CustomTrackableEventHandler : DefaultTrackableEventHandler
{
    private GameController _gameController;
    private bool _isVisited;
    private bool _isFirstFound;
    private PhotonView[] _photonViews;

    private void Awake()
    {
        _gameController = FindObjectOfType<GameController>();
    }

    protected override void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();

        if (SceneController.IsMobile)
        {
            _gameController.enabled = false;
        }

        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterOnTrackableStatusChanged(OnTrackableStatusChanged);
        }
    }

    private void OnTrackableStatusChanged(TrackableBehaviour.StatusChangeResult statusChangeResult)
    {
        m_PreviousStatus = statusChangeResult.PreviousStatus;
        m_NewStatus = statusChangeResult.NewStatus;

        HandleTrackableStatusChanged();
    }

    private void Update()
    {
        if (SceneController.IsMobile)
        {
            HidingPlayers();
        }
    }

    private void HidingPlayers()
    {
        if (PhotonNetwork.IsMasterClient || !gameObject.scene.isLoaded || _isVisited) return;

        _photonViews = FindPhotonViews();

        if (_photonViews.Length <= 0) return;
        _isVisited = true;

        SetActivePhotonViews(_photonViews, false);
    }

    private static PhotonView[] FindPhotonViews()
    {
        return FindObjectsOfType<PhotonView>();
    }

    private static void SetActivePhotonViews(IEnumerable<PhotonView> views, bool state)
    {
        foreach (var view in views)
        {
            view.gameObject.SetActive(state);
        }
    }

    protected override void OnTrackingLost()
    {
        if (mTrackableBehaviour)
        {
            var rendererComponents = mTrackableBehaviour.GetComponentsInChildren<Renderer>(true);
            var colliderComponents = mTrackableBehaviour.GetComponentsInChildren<Collider>(true);
            var canvasComponents = mTrackableBehaviour.GetComponentsInChildren<Canvas>(true);

            _photonViews = FindPhotonViews();
            SetActivePhotonViews(_photonViews, false);

            foreach (var component in rendererComponents)
            {
                component.enabled = false;
            }

            foreach (var component in colliderComponents)
            {
                component.enabled = false;
            }

            foreach (var component in canvasComponents)
            {
                component.enabled = false;
            }
        }

        OnTargetLost?.Invoke();
    }

    protected override void OnTrackingFound()
    {
        if (mTrackableBehaviour && SceneController.IsMobile)
        {
            if (!_isFirstFound)
            {
                var gameControllerTransform = _gameController.transform;
                var trackableTransform = mTrackableBehaviour.transform;

                gameControllerTransform.parent = trackableTransform.parent;
                gameControllerTransform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                gameControllerTransform.localPosition = Vector3.zero;
                gameControllerTransform.localRotation = Quaternion.identity;

                _gameController.enabled = true;

                _isFirstFound = true;
            }

            var rendererComponents = mTrackableBehaviour.GetComponentsInChildren<Renderer>(true);
            var colliderComponents = mTrackableBehaviour.GetComponentsInChildren<Collider>(true);
            var canvasComponents = mTrackableBehaviour.GetComponentsInChildren<Canvas>(true);

            SetActivePhotonViews(_photonViews, true);

            foreach (var component in rendererComponents)
            {
                component.enabled = true;
            }

            foreach (var component in colliderComponents)
            {
                component.enabled = true;
            }

            foreach (var component in canvasComponents)
            {
                component.enabled = true;
            }
        }

        OnTargetFound?.Invoke();
    }
}
