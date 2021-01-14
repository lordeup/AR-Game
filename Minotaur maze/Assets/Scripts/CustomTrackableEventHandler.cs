using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Vuforia;

public class CustomTrackableEventHandler : DefaultTrackableEventHandler
{
    private GameController _gameController;
    private bool _isVisited;
    private PhotonView[] _photonViews;

    private void Awake()
    {
        _gameController = FindObjectOfType<GameController>();
    }

    protected override void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
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
        // HidingPlayers();
    }

    private void HidingPlayers()
    {
        if (PhotonNetwork.IsMasterClient || !gameObject.scene.isLoaded || _isVisited) return;

        _photonViews = FindObjectsOfType<PhotonView>();

        if (_photonViews.Length <= 0) return;
        _isVisited = true;

        SetActivePhotonViews(_photonViews, false);
    }

    private static void SetActivePhotonViews(IEnumerable<PhotonView> views, bool state)
    {
        foreach (var view in views)
        {
            view.gameObject.SetActive(state);
        }
    }

    protected override void OnTrackingFound()
    {
        if (!mTrackableBehaviour) return;
        _gameController.enabled = true;

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

        OnTargetFound?.Invoke();
    }
}
