using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class CustomTrackableEventHandler : DefaultTrackableEventHandler
{
    private GameController gameController;
    private bool gameIsStarted;

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
    }

    protected override void Start()
    {
        gameIsStarted = false;
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        // RegisterTrackableEventHandler
        if (mTrackableBehaviour)
        {
            // mTrackableBehaviour.RegisterOnTrackableStatusChanged(OnTrackableStatusChanged);
            // mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

    protected override void OnTrackingFound()
    {
        if (!mTrackableBehaviour) return;
        if (!gameIsStarted)
        {
            gameIsStarted = true;
            gameController.enabled = true;
            // gameController.StartGame();
        }

        var rendererComponents = mTrackableBehaviour.GetComponentsInChildren<Renderer>(true);
        var colliderComponents = mTrackableBehaviour.GetComponentsInChildren<Collider>(true);
        var canvasComponents = mTrackableBehaviour.GetComponentsInChildren<Canvas>(true);

        // Enable rendering:
        foreach (var component in rendererComponents)
        {
            component.enabled = true;
        }

        // Enable colliders:
        foreach (var component in colliderComponents)
        {
            component.enabled = true;
        }

        // Enable canvas':
        foreach (var component in canvasComponents)
        {
            component.enabled = true;
        }
    }

    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        m_PreviousStatus = previousStatus;
        m_NewStatus = newStatus;

        Debug.Log("Trackable " + mTrackableBehaviour.TrackableName +
                  " " + mTrackableBehaviour.CurrentStatus +
                  " -- " + mTrackableBehaviour.CurrentStatusInfo);
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            OnTrackingFound();
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NO_POSE)
        {
            OnTrackingLost();
        }
        else
        {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            OnTrackingLost();
        }
    }
}
