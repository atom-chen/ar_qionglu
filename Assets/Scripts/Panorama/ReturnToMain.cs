using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class ReturnToMain : MonoBehaviour
{

    private Button Btn;
    public GameObject toastObj;
    public VideoPlayer vp;
    public VideoPlayer vp2;
    void Awake()
    {
        Btn = GetComponent<Button>();

        Btn.onClick.AddListener((() =>
        {
            SceneManager.LoadScene("Main");
        }));

        if (vp != null)
        {
            vp.url = GlobalInfo.VideoURL360;
            vp.Play();
        }

        if (vp2 != null)
        {
            vp2.url = GlobalInfo.VideoURL2D;
            vp2.Play();
            CoroutineWrapper.EXES(37f, () =>
            {
                if (vp != null)
                {    
                    vp2.Stop();
                    vp.url = GlobalInfo.VideoURL360;
                    vp.Play();
                    vp2.gameObject.SetActive(false);
                }
            });
        }
    }
    private bool isShooting;
    public void ShotPic()
    {
        if (!isShooting)
        {
            isShooting = true;
            ScreenshotManager.SaveScreenshot("Scan");
            toastObj.SetActive(true);
            CoroutineWrapper.EXES(1.5f, () =>
            {
                isShooting = false;
                toastObj.SetActive(false);
            });
        }
    }
}
