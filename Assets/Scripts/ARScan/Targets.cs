using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;
using Vuforia;
using DG.Tweening;

public class Targets : MonoBehaviour
{
    public int id;
    public VideoPlayer vp;
    public float videoTime;
    public AudioSource aud, aud2;
    public Material mat;
    public Texture[] target1tex;
    string scan_more_Path = "";
    string scan_ticket_Path = "";
    string scan_native_product_Path = "";
    string scan_Conjure_Path = "";

    public GameObject[] line;

    public Animator women;


    public GameObject[] points;
    public GameObject[] ships;
    void Start()
    {

    }
    public void ShowInfo()
    {
        if (ARScanManager.instance.IsTween)
        {
            Invoke("ShowInfo", 0.1f);
            return;
        }

        ARScanManager.instance.isScan = true;
        //foreach (var ChangeInfo in mainPageUI.curScenicInfo.ResourcesInfos)
        //{
        //    if (ChangeInfo.ResourcesKey == "scan_more")
        //    {
        //        scan_more_Path = ChangeInfo.LocalPath;
        //        Debug.Log(scan_more_Path);
        //    }
        //    else if (ChangeInfo.ResourcesKey == "scan_ticket")
        //    {
        //        scan_ticket_Path = ChangeInfo.LocalPath;
        //        Debug.Log(scan_ticket_Path);
        //    }
        //    else if (ChangeInfo.ResourcesKey == "scan_native_product")
        //    {
        //        scan_native_product_Path = ChangeInfo.LocalPath;
        //        Debug.Log(scan_native_product_Path);
        //    }else
        //        if(ChangeInfo.ResourcesKey== "scan_Conjure")
        //    {
        //        scan_Conjure_Path = ChangeInfo.LocalPath;
        //        Debug.Log(scan_Conjure_Path);
        //    }
        //}
        switch (id)
        {
            case 1:
                //扫一扫-泸山索滑道线路导览图
                StartCoroutine(target1());
                break;
            case 2:
                //扫一扫-梦里水乡导览图
                if (vp != null)
                {
                    //vp.url = scan_more_Path + "/shuixiang.mp4";
                    vp.Play();
                    women.gameObject.SetActive(true);
                    women.Play("start");
                }
                break;
            case 3:
                //扫一扫-邛海全景图（观鸟岛）
                speaktime = 3;
                StartCoroutine(PlayAnimOneShot());
                break;
            case 4:
                //扫一扫-小渔村
                StartCoroutine(target4());
                women.gameObject.SetActive(true);
                women.Play("start");
                aud.Play();
                break;
            case 5:
                //扫一扫-邛海游船价格公示栏
                StartCoroutine(target5());
                break;
            case 6:
                //扫一扫-赛波府酒店石头
                StartCoroutine(target6());
                women.gameObject.SetActive(true);
                women.Play("start");
                aud.Play();
                break;
            case 7:
                //扫一扫-稀客石头\
                StartCoroutine(showtex());
                women.gameObject.SetActive(true);
                women.Play("start");
                aud.Play();
                break;
            case 8:
                //扫一扫-月色风情小镇码头
                if (vp != null)
                {
                    //vp.url = scan_more_Path + "/matou.mp4";
                    vp.Play();
                    women.gameObject.SetActive(true);
                    women.Play("start");
                }
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
        }
    }
    public void HideInfo()
    {
        ARScanManager.instance.isScan = false;
        switch (id)
        {
            case 1:
                //扫一扫-泸山索滑道线路导览图
                StopCoroutine(target1());
                mat.mainTexture = target1tex[0];
                aud.Stop();
                line[0].SetActive(false);
                line[1].SetActive(false);
                break;
            case 2:
                //扫一扫-梦里水乡导览图
                if (vp != null)
                {
                    vp.Stop();
                    women.gameObject.SetActive(false);
                }
                break;
            case 3:
                //扫一扫-邛海全景图（观鸟岛）
                StopCoroutine(PlayAnimOneShot());
                women.gameObject.SetActive(false);
                women.Play("Idle");
                aud.Stop();
                break;
            case 4:
                //扫一扫-邛海全景图（瀛海亭）
                StopCoroutine(target4());
                women.gameObject.SetActive(false);
                women.Play("Idle");
                aud.Stop();
                break;
            case 5:
                //扫一扫-小渔村
                StopCoroutine(target5());
                Target5Lost();
                break;
            case 6:
                //扫一扫-赛波府酒店石头
                StopCoroutine(target6());
                women.gameObject.SetActive(false);
                women.Play("Idle");
                aud.Stop();
                break;
            case 7:
                //扫一扫-稀客石头
                StopCoroutine(showtex());
                women.gameObject.SetActive(false);
                women.Play("Idle");
                aud.Stop();
                break;
            case 8:
                //扫一扫-月色风情小镇码头
                if (vp != null)
                {
                    vp.Stop();
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
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(transform.position, -transform.up);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Debug.Log("碰撞对象: " + hit.collider.name);
                switch (id)
                {
                    case 7:
                        if (hit.collider.name == "id1")
                        {
                            webrequest.instance.LoadWeb("https://www.baidu.com/");
                        }
                        else if (hit.collider.name == "id2")
                        {
                            webrequest.instance.LoadWeb("https://www.jd.com/");
                        }
                        else if (hit.collider.name == "id3")
                        {
                            webrequest.instance.LoadWeb("https://www.taobao.com/");
                        }
                        break;
                }
            }
        }
    }
    public void ShowTicket(int num)
    {
        switch (num)
        {
            case 9:
                //vp.url = scan_ticket_Path + "/lushan.mp4";
                vp.Play();
                break;
            case 10:
                women.gameObject.SetActive(true);
                women.Play("start");
                //vp.url = scan_ticket_Path + "/shuixiang.mp4";
                vp.Play();
                break;
        }
    }
    public void HideTicket(int num)
    {
        switch (num)
        {
            case 9:
                if (vp.clip != null)
                    vp.Stop();
                break;
            case 10:
                women.gameObject.SetActive(false);
                women.Play("Idle");
                break;
        }

    }
    IEnumerator showtex()
    {
        for (int i = 0; i < target1tex.Length; i++)
        {
            mat.mainTexture = target1tex[i];
            yield return new WaitForSeconds(3);
        }
    }
    IEnumerator target1()
    {
        women.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        aud.Play();
        women.Play("speak");
        line[0].SetActive(false);
        line[1].SetActive(true);
        for (int i = 0; i < 4; i++)
        {
            mat.mainTexture = target1tex[i];
            yield return new WaitForSeconds(6);
        }
        yield return new WaitForSeconds(0);
        line[1].SetActive(false);
        line[0].SetActive(true);
        for (int i = 4; i < target1tex.Length; i++)
        {
            mat.mainTexture = target1tex[i];
            yield return new WaitForSeconds(5f);
        }
        women.Play("Idle");
    }

    private float speaktime = 1;
    IEnumerator PlayAnimOneShot()
    {
        women.gameObject.SetActive(true);
        women.Play("start");
        yield return new WaitForSeconds(1.5f);
        women.Play("speak");
        yield return new WaitForSeconds(speaktime);
        women.Play("Idle");
    }

    IEnumerator target4()
    {
        yield return new WaitForSeconds(2);
        women.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        women.Play("speak");
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
    }

    IEnumerator target5()
    {
        yield return new WaitForSeconds(1);
        women.gameObject.SetActive(true);
        women.Play("start");
        yield return new WaitForSeconds(1);
        women.Play("speak");
        points[0].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        ships[0].SetActive(true);
        ships[0].transform.DOLocalMove(new Vector3(-0.9f, 1.5f, 0), 3).SetEase(Ease.Linear);
        yield return new WaitForSeconds(3f);
        points[1].SetActive(true);
        yield return new WaitForSeconds(1f);
        ships[0].transform.DOLocalMove(new Vector3(1.3f, -2.1f, 0), 3).SetEase(Ease.Linear);
        yield return new WaitForSeconds(3f);
        points[2].SetActive(true);
        yield return new WaitForSeconds(1f);
        ships[0].transform.DOLocalMove(new Vector3(-0.9f, -2.8f, 0), 3).SetEase(Ease.Linear);
        yield return new WaitForSeconds(3f);
        yield return new WaitForSeconds(1f);
        points[1].SetActive(false);
        points[2].SetActive(false);

        ships[0].SetActive(false);
        ships[1].SetActive(true);
        ships[1].transform.DOLocalMove(new Vector3(-0.9f, 1.5f, 0), 3).SetEase(Ease.Linear);
        yield return new WaitForSeconds(3f);
        points[1].SetActive(true);
        yield return new WaitForSeconds(1f);
        ships[1].transform.DOLocalMove(new Vector3(1.3f, -2.1f, 0), 3).SetEase(Ease.Linear);
        yield return new WaitForSeconds(3f);
        points[2].SetActive(true);
        yield return new WaitForSeconds(1f);
        ships[1].transform.DOLocalMove(new Vector3(-0.9f, -2.8f, 0), 3).SetEase(Ease.Linear);
        yield return new WaitForSeconds(3f);
        yield return new WaitForSeconds(1f);
        points[1].SetActive(false);
        points[2].SetActive(false);

        ships[1].SetActive(false);
        ships[2].SetActive(true);
        ships[2].transform.DOLocalMove(new Vector3(1.3f, -2.1f, 0), 3).SetEase(Ease.Linear);
        yield return new WaitForSeconds(3f);
        points[2].SetActive(true);
        yield return new WaitForSeconds(1f);
        ships[2].transform.DOLocalMove(new Vector3(-0.9f, -2.8f, 0), 3).SetEase(Ease.Linear);
        yield return new WaitForSeconds(3f); 
        yield return new WaitForSeconds(1f);
        ships[2].SetActive(false);
        points[0].SetActive(false);
        points[1].SetActive(false);
        points[2].SetActive(false);
        yield return new WaitForSeconds(2f);
        Target5Lost();
        StartCoroutine(target5());

    }

    void Target5Lost()
    {
        points[0].SetActive(false);
        points[1].SetActive(false);
        points[2].SetActive(false);
        ships[0].SetActive(false);
        ships[1].SetActive(false);
        ships[2].SetActive(false);
    }
    IEnumerator target6()
    {
        yield return new WaitForSeconds(4);
        women.Play("speak");
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
    }
}
