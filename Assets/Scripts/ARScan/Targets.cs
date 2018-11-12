using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;

public class Targets : MonoBehaviour
{
    public int id;
    public VideoPlayer vp;
    public float videoTime;
    public AudioSource aud,audFX;
    public Material mat;
    public Texture[] target1tex;

    public GameObject[] line;

    public Animator women;


    public GameObject[] points;
    public GameObject[] ships;
    void Start()
    {

    }
    public void ShowInfo()
    {
        //if (ARScanManager.instance.IsTween)
        //{
        //    Invoke("ShowInfo", 0.1f);
        //    return;
        //}

        ARScanManager.instance.isScan = true;
        switch (id)
        {
            case 1:
                //扫一扫-泸山索滑道线路导览图
                cc1 = target1();
                StartCoroutine(cc1);
                break;
            case 2:
                //扫一扫-梦里水乡导览图
                cc2 = target2();
                StartCoroutine(cc2);
                break;
            case 3:
                //扫一扫-邛海全景图（观鸟岛）
                speaktime = 12;
                PlayAnimOneShot1 = PlayAnimOneShot();
                StartCoroutine(PlayAnimOneShot1);
                break;
            case 4:
                //扫一扫-小渔村
                cc4 = target4();
                StartCoroutine(cc4);
                break;
            case 5:
                //扫一扫-邛海游船价格公示栏
                cc5 = target5();
                StartCoroutine(cc5);
                // if (vp != null)
                // {
                //     vp.url = ARScanManager.scan_more_Path + "/youchuan.mp4";
                //     vp.Play();
                //     women.gameObject.SetActive(true);
                //     women.Play("start");
                // }
                break;
            case 6:
                //扫一扫-赛波府酒店石头
                cc6 = target6();
                StartCoroutine(cc6);
                break;
            case 7:
                //扫一扫-稀客石头\
                cc = showtex();
                StartCoroutine(cc);
                break;
            case 8:
                //扫一扫-月色风情小镇码头
                cc8 = target8();
                StartCoroutine(cc8);
                break;
            case 9:
                //泸山门票
                ShowTicket(9);
                break;
            case 10:
                //水乡门票
                ShowTicket(10);
                break;
            case 11:
                //特产瓷器
                CiqiManager.instance.OnTrackingFound();
                break;
            case 12:
                //特产苦荞茶
                KQCManager._Instance.OnTrackingFound();
                break;
            case 13:
                //特产杯垫
                BDManager._Instance.OnTrackingFound();
                break;
            case 14:
                bird3.instance.OnTrackerFound();
                break;
        }
    }
    public void HideInfo()
    {
        switch (id)
        {
            case 1:
                //扫一扫-泸山索滑道线路导览图
                if (cc1 != null)
                {
                    StopCoroutine(cc1);
                }
                mat.mainTexture = target1tex[0];
                aud.Stop();
                women.gameObject.SetActive(false);
                line[0].SetActive(false);
                line[1].SetActive(false);
                break;
            case 2:
                //扫一扫-梦里水乡导览图
                
                if (cc2 != null)
                {
                    StopCoroutine(cc2);
                }
                if (vp != null)
                {
                    vp.Stop();
                    women.Play("Idle");
                    women.gameObject.SetActive(false);
                }
                break;
            case 3:
                //扫一扫-邛海全景图（观鸟岛）
                if (PlayAnimOneShot1 != null)
                {
                    StopCoroutine(PlayAnimOneShot1);
                }
                if (ws != null)
                {
                    StopCoroutine(ws);
                }
                women.gameObject.SetActive(false);
                women.Play("Idle");
                aud.Stop();
                vp.Stop();
                vp.gameObject.SetActive(false);
                break;
            case 4:
                //扫一扫-邛海全景图（瀛海亭）
                if (cc4 != null)
                {
                    StopCoroutine(cc4);
                }
                women.gameObject.SetActive(false);
                women.Play("Idle");
                aud.Stop();
                break;
            case 5:
                //扫一扫-小渔村
                
                if (cc5 != null)
                {
                    StopCoroutine(cc5);
                }
                if (vp != null)
                {
                    vp.Stop();
                    women.Play("Idle");
                    women.gameObject.SetActive(false);
                }
                break;
            case 6:
                //扫一扫-赛波府酒店石头
                
                if (cc6 != null)
                {
                    StopCoroutine(cc6);
                }
 
                women.gameObject.SetActive(false);
                women.Play("Idle");
                aud.Stop();
                break;
            case 7:
                //扫一扫-稀客石头
                if (cc != null)
                {
                    StopCoroutine(cc);
                }

                women.gameObject.SetActive(false);
                women.Play("Idle");
                aud.Stop();
                break;
            case 8:
                //扫一扫-月色风情小镇码头
                
                if (cc8 != null)
                {
                    StopCoroutine(cc8);
                }

                if (vp != null)
                {
                    vp.Stop();
                    women.Play("Idle");
                    women.gameObject.SetActive(false);
                }
                break;
            case 9:
                //泸山门票
                HideTicket(9);
                break;
            case 10:
                //水乡门票
                HideTicket(10);
                break;
            case 11:
                CiqiManager.instance.OnTrackingLost();
                break;
            case 12:
                //特产苦荞茶
                KQCManager._Instance.OnTrackingLost();
                break;
            case 13:
                //特产杯垫
                BDManager._Instance.OnTrackingLost();
                break;
            case 14:
                bird3.instance.OnTrackerLost();
                break;
        }
    }
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
#if UNITY_EDITOR
            if (EventSystem.current.IsPointerOverGameObject())

#elif UNITY_ANDROID || UNITY_IPHONE
             if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
#endif
                Debug.Log("当前触摸在UI上");
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    Debug.Log("碰撞对象: " + hit.collider.name);
                    switch (id)
                    {
                        case 3:
                            if (hit.collider.name == "id1")
                            {
                                videoTime = 15;
                                PlayVideo(ARScanManager.scan_more_Path + "/id1.mp4");
                            }
                            else if (hit.collider.name == "id2")
                            {
                                videoTime = 15;
                                PlayVideo(ARScanManager.scan_more_Path + "/id2.mp4");
                            }
                            else if (hit.collider.name == "id3")
                            {
                                videoTime = 15;
                                PlayVideo(ARScanManager.scan_more_Path + "/id3.mp4");
                            }
                            else if (hit.collider.name == "id4")
                            {
                                videoTime = 15;
                                PlayVideo(ARScanManager.scan_more_Path + "/id4.mp4");
                            }
                            else
                            {
                                if (isVideo && id == 3)
                                {
                                    
                                    if (ws != null)
                                    {
                                        StopCoroutine(ws);
                                    }
    
                                    vp.Stop();
                                    vp.gameObject.SetActive(false);
                                }
                            }
                            break;
                    }
                }
                else
                {
                    if (isVideo && id == 3)
                    {
                        if (ws != null)
                        {
                            StopCoroutine(ws);
                        }
      
                        vp.Stop();
                        vp.gameObject.SetActive(false);
                    }
                }
            }
        }
        

    }

    private IEnumerator ws;
    private bool isVideo;
    private void PlayVideo(string url)
    {
        isVideo = true;
        vp.gameObject.SetActive(true);
        vp.url = url;
        vp.Play();
        ws = womenSpeak();
        StartCoroutine(ws);
    }

    IEnumerator womenSpeak()
    {
        women.Play("speak");
        yield return new WaitForSeconds(videoTime);
        women.Play("Idle");
    }
    
    public void ShowTicket(int num)
    {
        switch (num)
        {
            case 9:
                if (File.Exists(ARScanManager.scan_ticket_Path + "/lushan.mp4"))
                {
                    vp.url = ARScanManager.scan_ticket_Path + "/lushan.mp4";
                    vp.Play();
                }
                break;
            case 10:
                t1 = ticket();
                StartCoroutine(t1);
                break;
        }
    }
    public void HideTicket(int num)
    {
        switch (num)
        {
            case 9:
                if (vp != null)
                    vp.Stop();
                break;
            case 10:
                
                if (t1 != null)
                {
                    StopCoroutine(t1);
                }

                if (vp != null)
                    vp.Stop();
                women.gameObject.SetActive(false);
                women.Play("Idle");
                break;
        }

    }

    IEnumerator ticket()
    {
        women.gameObject.SetActive(true);
        women.Play("start");
        audFX.Play();
        yield return new WaitForSeconds(2);
        women.Play("speak");
        vp.url = ARScanManager.scan_ticket_Path + "/shuixiang2.mp4";
        vp.Play();
    }
    private IEnumerator  t1,cc,cc1,PlayAnimOneShot1,cc2,cc4,cc5,cc6,cc8;
    IEnumerator showtex()
    {
        women.gameObject.SetActive(true);
        women.Play("start");
        audFX.Play();
        yield return new WaitForSeconds(2);
        women.Play("speak");
        aud.Play();
        for (int i = 0; i < target1tex.Length; i++)
        {
            mat.mainTexture = target1tex[i];
            yield return new WaitForSeconds(4.3f);
        }
        women.Play("Idle");

        yield return new WaitForSeconds(3);
        StartCoroutine(cc);
    }
    IEnumerator target1()
    {
        women.gameObject.SetActive(true);
        women.Play("start");
        audFX.Play();
        yield return new WaitForSeconds(2);
        women.Play("speak");
        aud.Play();
        line[0].SetActive(false);
        line[1].SetActive(true);
        for (int i = 0; i < 4; i++)
        {
            mat.mainTexture = target1tex[i];
            yield return new WaitForSeconds(3.5f);
        }
        yield return new WaitForSeconds(0);
        line[1].SetActive(false);
        line[0].SetActive(true);
        for (int i = 4; i < target1tex.Length; i++)
        {
            mat.mainTexture = target1tex[i];
            yield return new WaitForSeconds(4.6f);
        }
        women.Play("Idle");
        line[1].SetActive(false);
        line[0].SetActive(false);

        yield return new WaitForSeconds(3);
        StartCoroutine(cc1);
    }
    
    IEnumerator target2()
    {        
        women.gameObject.SetActive(true);
        women.Play("start");
        audFX.Play();
        yield return new WaitForSeconds(2);
        if (vp!=null)
        { 
            vp.url = ARScanManager.scan_more_Path + "/shuixiang1.mp4";
            vp.Play();
        }
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.4f);
        women.Play("speak");
        yield return new WaitForSeconds(videoTime);
        women.Play("Idle");

        yield return new WaitForSeconds(3);
        StartCoroutine(cc2);
    }
    
    private float speaktime = 1;
    IEnumerator PlayAnimOneShot()
    {
        women.gameObject.SetActive(true);
        women.Play("start");
        audFX.Play();
        yield return new WaitForSeconds(1.5f);
        women.Play("speak");
        aud.Play();
        yield return new WaitForSeconds(speaktime);
        women.Play("Idle");
    }

    IEnumerator target4()
    {
        yield return new WaitForSeconds(2);
        women.gameObject.SetActive(true);
        women.Play("start");
        audFX.Play();
        yield return new WaitForSeconds(2);
        women.Play("speak");
        aud.Play();
        for (int i = 0; i < 4; i++)
        {
            mat.mainTexture = target1tex[i];
            yield return new WaitForSeconds(6);
        }
        yield return new WaitForSeconds(0);
        for (int i = 4; i < 8; i++)
        {
            mat.mainTexture = target1tex[i];
            yield return new WaitForSeconds(4.2f);
        }
        yield return new WaitForSeconds(0);
        for (int i = 8; i < target1tex.Length; i++)
        {
            mat.mainTexture = target1tex[i];
            yield return new WaitForSeconds(4.2f);
        }
        women.Play("Idle");

        yield return new WaitForSeconds(3);
        StartCoroutine(cc4);
    }
    
    IEnumerator target5()
    {        
        women.gameObject.SetActive(true);
        women.Play("start");
        audFX.Play();
        yield return new WaitForSeconds(1);
        if (vp!=null)
        { 
            vp.url = ARScanManager.scan_more_Path + "/youchuan.mp4";
            vp.Play();
        }
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.4f);
        women.Play("speak");
        yield return new WaitForSeconds(videoTime);
        women.Play("Idle");

        yield return new WaitForSeconds(3);
        StartCoroutine(cc8);
    }
    IEnumerator target6()
    {
        yield return new WaitForSeconds(1);
        women.gameObject.SetActive(true);
        women.Play("start");
        audFX.Play();
        yield return new WaitForSeconds(2);
        women.Play("speak");
        aud.Play();
        for (int i = 0; i < target1tex.Length; i++)
        {
            mat.mainTexture = target1tex[i];
            yield return new WaitForSeconds(4.3f);
        }
        women.Play("Idle");

        yield return new WaitForSeconds(3);
        StartCoroutine(cc6);
    }
    
    IEnumerator target8()
    {        
        women.gameObject.SetActive(true);
        women.Play("start");
        audFX.Play();
        yield return new WaitForSeconds(1);
        if (vp!=null)
        { 
            vp.url = ARScanManager.scan_more_Path + "/matou.mp4";
            vp.Play();
        }
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.4f);
        women.Play("speak");
        yield return new WaitForSeconds(videoTime);
        women.Play("Idle");

        yield return new WaitForSeconds(3);
        StartCoroutine(cc8);
    }
}
