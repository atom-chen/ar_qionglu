using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using DG.Tweening;
public class KQCManager : MonoBehaviour
{
    public static KQCManager _Instance;
    public GameObject videoGo, roleGo;
    VideoPlayer videoPlayer;
    public AudioSource aud,audFX;
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
        fo = Found();
        StartCoroutine(fo);
    }
    public void OnTrackingLost()
    {
        Lost();
    }

    private IEnumerator fo;


    IEnumerator Found()
    {      
        roleGo.transform.parent.gameObject.SetActive(true);
        roleGo.GetComponent<Animator>().Play("start");
        audFX.Play();
        yield return new WaitForSeconds(2.0f);
        aud.Play();   
        roleGo.GetComponent<Animator>().Play("speak");
        //   roleGo.GetComponent<Animator>().Play("speak");
        yield return new WaitForSeconds(SpeakTime - 1f);
        videoGo.gameObject.SetActive(true);
        roleGo.GetComponent<Animator>().Play("Idle");
        aud.Stop();
        videoGo.transform.DOLocalMove(new Vector3(-1f, 0.1f, 0.18f), 1f);


        if (File.Exists(ARScanManager.scan_native_product_Path + "/kqc.mp4"))
        {
            videoPlayer.url = ARScanManager.scan_native_product_Path + "/kqc.mp4";
            videoPlayer.Play();
        }

        yield return new WaitForSeconds(VideoTime);
    }
    void Lost()
    {
        if (fo != null)
        {
            StopCoroutine(fo);
        }
        
        aud.Stop();
        if (Check(videoPlayer, roleGo.GetComponent<Animator>()))
        {
            videoPlayer.Stop();
            roleGo.GetComponent<Animator>().Play("Zero");
        }
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
