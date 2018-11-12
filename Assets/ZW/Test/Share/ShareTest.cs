using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShareTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void  Share()
    {



        ShareScriptsBase.Instance.ShareVideo(PublicAttribute.LocalFilePath+"Push/1.mp4");
    }
}
