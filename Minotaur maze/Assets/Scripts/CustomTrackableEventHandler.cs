using UnityEngine;
using Vuforia;

public class CustomTrackableEventHandler : DefaultTrackableEventHandler
{
    private GameController _gameController;
    private PlaneFinderBehaviour _planeFinderBehaviour;

    private void Awake()
    {
        _gameController = FindObjectOfType<GameController>();
        _planeFinderBehaviour = FindObjectOfType<PlaneFinderBehaviour>();
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

    protected override void OnTrackingFound()
    {
        if (!mTrackableBehaviour) return;
        _gameController.enabled = true;
        _gameController.transform.localPosition = _planeFinderBehaviour.transform.localPosition;

        var rendererComponents = mTrackableBehaviour.GetComponentsInChildren<Renderer>(true);
        var colliderComponents = mTrackableBehaviour.GetComponentsInChildren<Collider>(true);
        var canvasComponents = mTrackableBehaviour.GetComponentsInChildren<Canvas>(true);

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
