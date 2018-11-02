using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ports
{
    public string address; //地址
    public string portnum; //端口号
}
public class PortsStatus
{
    public connect[] allports;
}

public class connect
{
    public connect()
    {
        address = "";
        portnum = "";
    }
    public string address; //地址
    public string portnum; //端口号
}
public class initScene : MonoBehaviour
{
    public static initScene instance;
    private List<Ports> allport = new List<Ports>();
    public GameObject guide,ads;

    public RectTransform splash;
    public static string localFilePath;

    public GameObject root;
    public webrequest web;
    public static int FirstEnter
    {
        set {PlayerPrefs.SetInt("FirstEnter", value);}
        get { return PlayerPrefs.GetInt("FirstEnter");}
    }
    void Awake()
    {
        instance = this;
        
        if (FirstEnter == 0)
        {
            ads.SetActive(false);
        }
    }

    public bool isLookingAds;
    IEnumerator Start()
    {
        //屏幕常亮
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        if (GlobalInfo.LastAdsUrl.Length > 5 && GlobalInfo.LastAdsImgPath.Length > 5)
        {
            ads.GetComponent<Button>().onClick.AddListener(delegate
            {
                isLookingAds = true;
                web.LoadWebSetTitle(GlobalInfo.LastAdsUrl,"特产详情"); 
            });
            StartCoroutine(LoadImgFromCache(GlobalInfo.LastAdsImgPath, ads.GetComponent<RawImage>()));
            
            yield return new WaitForEndOfFrame();
            
            HttpManager.Instance.GetAds((b =>
            {
                if (b)
                {
                    foreach (var page in JsonClass.Instance.AdsPages)
                    {
                        HttpManager.Instance.Download(page.Thumbnail, (() =>
                        {                       
                            GlobalInfo.LastAdsUrl = page.address;
                            GlobalInfo.LastAdsImgPath = page.Thumbnail.localPath;
                        }));
                    }
                }
            }));       
        }
        else
        {
            HttpManager.Instance.GetAds((b =>
            {
                if (b)
                {
                    foreach (var page in JsonClass.Instance.AdsPages)
                       {            
                           ads.GetComponent<Button>().onClick.AddListener(delegate
                            {
                              if (page.address.Length > 6)
                              {
                                isLookingAds = true;
                                  web.LoadWebSetTitle(page.address,"特产详情"); 
                              }    
                          });
                          HttpManager.Instance.Download(page.Thumbnail, (() =>
                           {
                               GlobalInfo.LastAdsUrl = page.address;
                               GlobalInfo.LastAdsImgPath = page.Thumbnail.localPath;
                               StartCoroutine(LoadImgFromCache(GlobalInfo.LastAdsImgPath, ads.GetComponent<RawImage>()));
                           }));
                      }
                 }
             }));            
        }
        yield return new WaitForEndOfFrame();
        root.SetActive(true);
        yield return new WaitForEndOfFrame();
        // guide.SetActive(true);
        // splash.gameObject.SetActive(false);

        yield return new WaitForSeconds(3);
        if (FirstEnter==0)
        {
            FirstEnter = 1;
        }
        else if (!isLookingAds)
        {
            
            EnterMain();
        }
    }
    public void EnterMain()
    {
        FirstEnter = 1;
        HttpManager.Instance.GetUserInfoByToken((b =>
        {
            if (b)
            {
                UnityHelper.LoadNextScene("main");
            }
            else
            {
                ScenesManager.Instance.LoadLoginScene();
            }
        }));
    }
    public void HideAds()
    {
        EnterMain();
    }

    public void GetState()
    {

    }

    private IEnumerator LoadImgFromCache(string imgURl, RawImage img)
    {
        if (CheckCacheUrlIsExit(imgURl))
        {
            Texture2D tex = new Texture2D(256, 256, TextureFormat.ARGB32, false);
            img.texture = tex;
            HttpBase.Download(imgURl, ((request, downloaded, length) => { }), ((request, response)
                =>
            {
                if (response == null || !response.IsSuccess)
                {
                    DebugManager.Instance.LogError("请求失败！");
                    return;
                }
                tex.LoadImage(response.Data);
            }));
        }
        else
        {
            yield break;
        }
    }
    /// <summary>
    /// 检查文件是否存在
    /// </summary>
    /// <param name="imgURl"></param>
    /// <returns></returns>
    private bool CheckCacheUrlIsExit(string imgURl)
    {
        return true;
    }
    //private IEnumerator LoadAssets2()
    //{
    //    string path = Application.persistentDataPath + "/DownloadFile/Panorama/1/shajin.vsz";
    //    Debug.Log(path);
    //    Debug.Log(File.Exists(path));
    //    int index1 = path.LastIndexOf("/");
    //    int index2 = path.LastIndexOf(".");
    //    string suffix = path.Substring(index1 + 1, index2 - index1 - 1);
    //    WWW bundle = WWW.LoadFromCacheOrDownload("file:///" + path, 0);
    //    yield return bundle;
    //    Debug.Log(bundle.size);
    //    var data = bundle.assetBundle;
    //    SceneManager.LoadScene("shajin");
    //}
}
