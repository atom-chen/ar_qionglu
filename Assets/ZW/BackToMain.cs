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
  
        switch (SceneManager.GetActiveScene().name)
        {
            case "yiyou":
                GroundPlaneUI.Instance.BackBtnClick();
                break;
            case   "wikiSLAM":
                WikiSLAMUIController.Instance.BackBtnClick();
          
                break;
            case "Track":
                TrackUIManager.Instance.BackBtnClick();
       
                break;
            default:
       
                break;
        }
      
        }



    
}
