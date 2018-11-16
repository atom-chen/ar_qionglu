using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMain : MonoBehaviour {


	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackBtnClick();
        }
	}



    public  void BackBtnClick()
    {
        //if (FirstUseTipManager.Instance.isTipTour==true)
        //{
        //    return;
        //}
        switch (SceneManager.GetActiveScene().name)
        {
            case "yiyou":
                GroundPlaneUI.Instance.BackBtnClick();
                break;
#if UNITY_ANDROID
            case "wikiSLAM":
                WikiSLAMUIController.Instance.BackBtnClick();

                break;

#endif

            case "Track":
                TrackUIManager.Instance.BackBtnClick();
       
                break;
            default:
       
                break;
        }
      
        }



    
}
