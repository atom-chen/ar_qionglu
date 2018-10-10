using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Vuforia;
public class ticket : MonoBehaviour , ITrackableEventHandler
{
    protected TrackableBehaviour mTrackableBehaviour;
    public VideoPlayer vp;
    public float videoTime;
    public GameObject GuideBtnList;
    public int id;

	void Awake () {
	    Screen.orientation = ScreenOrientation.LandscapeLeft;
    }
    protected virtual void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
    }

    protected virtual void OnDestroy()
    {
        if (mTrackableBehaviour)
            mTrackableBehaviour.UnregisterTrackableEventHandler(this);
    }

    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
            OnTrackingFound();
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NO_POSE)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
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

    public void OnTrackingFound()
    {
        switch (id)
        {
            case 1:
                if (vp.clip != null)
                    vp.Play();
                StartCoroutine(ShowGuideBtnList(videoTime));
                break;
            case 2:
                break;
        }
    }
    public void OnTrackingLost()
    {
        switch (id)
        {
            case 1:
                if (vp.clip != null)
                    vp.Stop();
                StopCoroutine(ShowGuideBtnList(videoTime));
                break;
            case 2:
                break;
        }

    }

    IEnumerator ShowGuideBtnList(float second)
    {
        yield return new WaitForSeconds(second);
        //GuideBtnList.SetActive(true);
    }
}
