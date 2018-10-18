using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using DG.Tweening;
public class KQCManager : MonoBehaviour
{
    public static KQCManager _Instance;
    public GameObject videoGo, roleGo;
    VideoPlayer videoPlayer;
    public AudioSource aud;
    float SpeakTime = 16f, VideoTime = 26f;
    void Awake()
    {
        _Instance = this;

        videoPlayer = videoGo.GetComponentInChildren<VideoPlayer>();
        videoGo.gameObject.SetActive(false);

        videoPlayer.url = ARScanManager.scan_native_product_Path + "kqc.mp4";
    }
    private void Start()
    {
    }

    public void OnTrackingFound()
    {
        StopAllCoroutines();
        StartCoroutine(Found());
    }
    public void OnTrackingLost()
    {
        Lost();
    }



    IEnumerator Found()
    {
        string scan_native_product = "";
        foreach (var ChangeInfo in mainPageUI.curScenicInfo.ResourcesInfos)
        {
            if (ChangeInfo.ResourcesKey == "scan_native_product")
            {
                scan_native_product = ChangeInfo.LocalPath;
                Debug.Log(scan_native_product);
            }
        }


        CiqiManager.instance.LostEvent();
        BDManager._Instance.OnTrackingLost();
        roleGo.transform.parent.gameObject.SetActive(true);
        AudioManager.instance.StopAll();
        roleGo.GetComponent<Animator>().Play("start");
        aud.Play();

        yield return new WaitForSeconds(1.0f);

        //   roleGo.GetComponent<Animator>().Play("speak");
        yield return new WaitForSeconds(SpeakTime - 1f);
        videoGo.gameObject.SetActive(true);
        roleGo.GetComponent<Animator>().Play("Idle");
        AudioManager.instance.StopAll();
        videoGo.transform.DOLocalMove(new Vector3(-1f, 0.1f, 0.18f), 1f);



        videoPlayer.url = scan_native_product + "/kqc.mp4";
        videoPlayer.Play();
        yield return new WaitForSeconds(VideoTime);
    }
    void Lost()
    {
        StopAllCoroutines();
        aud.Stop();
        if (Check(videoPlayer, roleGo.GetComponent<Animator>()))
        {
            videoPlayer.Stop();
            roleGo.GetComponent<Animator>().Play("Zero");
        }
        AudioManager.instance.StopAll();
        videoGo.transform.localPosition = new Vector3(0f, -1f, 0f);

        videoGo.gameObject.SetActive(false);
    }
    private bool Check(params Component[] com)
    {
        foreach (Component item in com)
        {
            if (item == null)
            {
                return false;
            }
        }
        return true;
    }


}
