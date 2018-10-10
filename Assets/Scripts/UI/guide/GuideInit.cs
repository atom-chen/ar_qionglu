using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideInit : MonoBehaviour {

	// Use this for initialization
	void Awake ()
	{
	    DownloadProp.Instance.UpdateCategoryInfo();
	    DownloadProp.Instance.UpdateComponentInfo();
	}

}
