using ElviraFrame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackShowImage : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.GetComponentInChildren<Button>().onClick.AddListener(CancelClick);
    }

    private void CancelClick()
    {
        FingerTouchEL.Instance.targetGameObject = null;
        Destroy(this.gameObject);

    }
}
