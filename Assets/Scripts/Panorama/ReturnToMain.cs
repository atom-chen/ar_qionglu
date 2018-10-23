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
            vp.url = PublicAttribute.LocalFilePath + "Panorama/1/jiulonghanbai.mp4";
            vp.Play();
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
