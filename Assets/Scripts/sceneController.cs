using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class sceneController : MonoBehaviour
{
    public Camera firstPersonCamera;
    void QuitOnConnectionErrors()
    {
        if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
        {
            StartCoroutine(CodelabUtils.ToastAndExit("Camera permission is needed to run this application.", 5));

        }

        else if (Session.Status.IsError())
        {
            StartCoroutine(CodelabUtils.ToastAndExit("ARCore encountered a problem connecting. Please restart the app.", 5));
        }
    }

    private void Start()
    {
        QuitOnConnectionErrors();
    }

    private void Update()
    {
        if(Session.Status != SessionStatus.Tracking)
        {
            int lostTrakingTimeout = 15;
            Screen.sleepTimeout = lostTrakingTimeout;
            return;

        }
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        processTouches();
    }

     void processTouches()
    {
        Touch touch;
        if(Input.touchCount !=1|| (touch = Input.GetTouch(0)).phase !=TouchPhase.Began)
        {
            return;
        }
        TrackableHit hit;
        TrackableHitFlags raycastFilter = 
            TrackableHitFlags.PlaneWithinBounds |
            TrackableHitFlags.PlaneWithinPolygon;
        if(Frame.Raycast(touch.position.x, touch.position.y,raycastFilter, out hit))
        {
            SetSelectedPlane (hit.Trackable as DetectedPlane);
        }



    }

    void SetSelectedPlane(DetectedPlane selectedPlane)
    {
        Debug.Log("Selected plane centered at " + selectedPlane.CenterPose.position);
    }
}