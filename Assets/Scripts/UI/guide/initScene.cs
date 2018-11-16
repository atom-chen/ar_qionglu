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

public class initScene : MonoBehaviour
{
    public static initScene instance;
    public GameObject ads;

    public GameObject root;
    public webrequest web;

    public RectTransform[] pages;

    public GameObject zipPanel;
    public static string FirstEnter
    {
        set {PlayerPrefs.SetString("FirstEnter", value);}
        get { return PlayerPrefs.GetString("FirstEnter");}
    }
    void Awake()
    {
        instance = this;
        
        if (FirstEnter != GlobalInfo.APPversion)
        {
            ads.SetActive(false);
        }
    }

    public bool isLookingAds;
    private static string isUnzipOver
    {
        set {PlayerPrefs.SetString("isUnzipOver", value);}
        get { return PlayerPrefs.GetString("isUnzipOver");}
    }
    IEnumerator Start()
    {
#if UNITY_IOS || UNITY_IPHONE
        if (isUnzipOver != "true" && FirstEnter != GlobalInfo.APPversion)
        {
            unzip._inst.unzipres((b =>
            {
                if (b)
                {
                    isUnzipOver = "true";
                }
                else
                {
                    isUnzipOver = "false";
                }
            }));
        }
#endif
        
        //屏幕常亮
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        DownloadProp.Instance.UpdateCategoryInfo();
        DownloadProp.Instance.UpdateComponentInfo();
        
        for (int i = 0; i < pages.Length; i++)
        {
#if UNITY_ANDROID || UNITY_EDITOR
            pages[i].sizeDelta=new Vector2(Screen.width,Screen.height);
            pages[i].anchoredPosition=new Vector2(0,1920-Screen.height);
#elif UNITY_IOS || UNITY_IPHONE

#endif
        }
        if (GlobalInfo.LastAdsUrl.Length > 5 && GlobalInfo.LastAdsImgPath.Length > 5 && File.Exists(GlobalInfo.LastAdsImgPath))
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
        
#if UNITY_ANDROID || UNITY_EDITOR
        yield return new WaitForSeconds(3);
        if (FirstEnter != GlobalInfo.APPversion)
        {
            FirstEnter = GlobalInfo.APPversion;
        }
        else if (!isLookingAds)
        {
            
            EnterMain();
        }
#elif UNITY_IOS || UNITY_IPHONE
        if (isUnzipOver=="true")
        {
            yield return new WaitForSeconds(3);
            if (FirstEnter != GlobalInfo.APPversion)
            {
                FirstEnter = GlobalInfo.APPversion;
            }
            else if (!isLookingAds)
            {
            
                EnterMain();
            }
        }
        else
        {
            zipPanel.SetActive(true);
            canchange = true;
        }
#endif
      
    }

    private bool canchange = false;
    private void Update()
    {
        if (isUnzipOver=="true" && canchange)
        {
            canchange = false;
            EnterMain();
        }
    }

    public void EnterMain()
    {
        FirstEnter = GlobalInfo.APPversion;
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
#if UNITY_ANDROID || UNITY_EDITOR
        EnterMain();
#elif UNITY_IOS || UNITY_IPHONE
        if (isUnzipOver=="true")
        {
            EnterMain();
        }
        else
        {
            zipPanel.SetActive(true);
            canchange = true;
        }
#endif
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
}
