using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using cn.sharesdk.unity3d;
using UnityEngine.UI;
using com.moblink.unity3d;
using LitJson;
using UnityEngine.SceneManagement;
/// <summary>
/// 分享的基类
/// </summary>
public class ShareScriptsBase : SingletonMono<ShareScriptsBase>
{
    [HideInInspector]
    public ShareSDK ssdk;

    // Use this for initialization
    void Start()
    {
        if (ssdk==null)
        {
            ssdk = transform.GetComponent<ShareSDK>();

        }
        ssdk.authHandler = OnAuthResultHandler;
        ssdk.shareHandler = OnShareResultHandler;
        ssdk.showUserHandler = OnGetUserInfoResultHandler;

    }

    /// <summary>
    /// 回调获得类是Token的信息
    /// </summary>
    /// <param name="reqID"></param>
    /// <param name="state"></param>
    /// <param name="type"></param>
    /// <param name="result"></param>
    void OnAuthResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
    {
        if (state == ResponseState.Success)
        {
            print("authorize success !" + "Platform :" + type);
        }
        else if (state == ResponseState.Fail)
        {
#if UNITY_ANDROID
            print("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
#elif UNITY_IPHONE
            print("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
#endif
        }
        else if (state == ResponseState.Cancel)
        {
            print("cancel !");
        }
    }
    /// <summary>
    /// 回调获得用户账号信息
    /// </summary>
    /// <param name="reqID"></param>
    /// <param name="state"></param>
    /// <param name="type"></param>
    /// <param name="result"></param>
    void OnGetUserInfoResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
    {
        if (state == ResponseState.Success)
        {
            print("get user info result :");
        
            print("Get userInfo success !Platform :" + type);
        }
        else if (state == ResponseState.Fail)
        {
#if UNITY_ANDROID
            print("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
#elif UNITY_IPHONE
            print("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
#endif
        }
        else if (state == ResponseState.Cancel)
        {
            print("cancel !");
        }
    }
    /// <summary>
    /// 分享的回调
    /// </summary>
    /// <param name="reqID"></param>
    /// <param name="state"></param>
    /// <param name="type"></param>
    /// <param name="result"></param>
    void OnShareResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
    {
        if (state == ResponseState.Success)
        {
            print("share successfully - share result :");
        }
        else if (state == ResponseState.Fail)
        {
#if UNITY_ANDROID
            print("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
#elif UNITY_IPHONE
            print("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
#endif

        }
        else if (state == ResponseState.Cancel)
        {
            print("cancel !");
        }
    }


    /// <summary>
    /// 分享Link,
    /// url：图片的路径 
    /// modelName：模型本身的名字
    ///  modelText：模型上写的字
    ///  modelTransform：模型的Transform
    ///  
    /// 
    /// </summary>
    private  void ShareLink(string imagURL)
    {
        if (imagURL == null) return;




        MobLinkScene scene = new MobLinkScene("","",YiyouStaticDataManager.Instance.GetHashtable());
        MobLink.getMobId(scene, (mobid) =>
        {
            if (mobid != null && mobid.Length > 0)
            {
                string linkUrl = GlobalParameter.LinkWebURL + mobid;
                ShareContent content = new ShareContent();
                content.SetImagePath(imagURL);
                content.SetText("AR凉山游1");
                content.SetComment("AR凉山游2");
                content.SetSite("AR凉山游3");
                content.SetSiteUrl(linkUrl);
                content.SetTitle("AR凉山游4");
                content.SetTitleUrl(linkUrl);
                content.SetShareType(ContentType.Image);
                //通过分享菜单分享
                ssdk.ShowPlatformList(null, content, 100, 100);
            }

        });
    }



    /// <summary>
    /// 分享图片或者视频
    /// </summary>
    /// <param name="path"></param>
    private void ShareImage(string path)
    {
        if (path == null) return;
        Debug.Log("path===" + path);
        ShareContent content = new ShareContent();
        content.SetImagePath(path);
        content.SetText("AR凉山游1");
        content.SetComment("AR凉山游2");
        content.SetSite("AR凉山游3");
        content.SetSiteUrl("http://www.visizen.com");

        content.SetTitle("AR凉山游4");
        content.SetTitleUrl("http://www.visizen.com");

        content.SetShareType(ContentType.Image);
    

        //通过分享菜单分享
        ssdk.ShowPlatformList(null, content, 100, 100);
    }

    internal void Share(string savedPath)
    {
        if (GlobalParameter.isNeedRestore)
        {
            ShareLink(savedPath);

        }
        else
        {
            ShareImage(savedPath);
        }
    }
}
