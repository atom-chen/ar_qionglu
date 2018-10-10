using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class initScene : MonoBehaviour
{
    public GameObject guide,ads;

    public RectTransform splash;
    // Use this for initialization
    IEnumerator Start ()
	{
        yield return new WaitForSeconds(3);
        guide.SetActive(true);
        splash.gameObject.SetActive(false);
        yield return new WaitForSeconds(5);
	    ads.SetActive(false);
    }

    public void HideAds()
    {
        ads.SetActive(false);
    }

    public void GetState()
    {

    }
}
