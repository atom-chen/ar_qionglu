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
    private  bool ShareLink(string imagURL)
    {
        if (imagURL == null) return false; ;




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
        return true;
    }



    /// <summary>
    /// 分享图片
    /// </summary>
    /// <param name="path"></param>
    private bool ShareImage(string path)
    {
        if (path == null) return  false;
        PlatformType[] plats = new PlatformType[4];
        plats[0] = PlatformType.WeChat;
        plats[1] = PlatformType.WeChatMoments;
        plats[2] = PlatformType.QQ;
        plats[3] = PlatformType.SinaWeibo;
        ssdk.DisableSSO(false);
        ShareContent content = new ShareContent();
        content.SetText("视觉美景+智能呈现  只留精彩，不留遗憾");//智能交互宣传
        content.SetImagePath(path);
        content.SetTitle("AR游");//AR游
        content.SetShareType(ContentType.Image);
        content.SetTitleUrl("http://download.vszapp.com");
        content.SetUrl("http://download.vszapp.com");

        ShareContent qqParams = new ShareContent();
        qqParams.SetImagePath(path);
        qqParams.SetShareType(ContentType.Image);
        qqParams.SetShareContentCustomize(PlatformType.QQ, qqParams);

        ShareContent wechatParams = new ShareContent();
        wechatParams.SetImagePath(path);
        wechatParams.SetTitle("AR游");//AR游
        wechatParams.SetShareType(ContentType.Image);
        wechatParams.SetShareContentCustomize(PlatformType.WeChat, wechatParams);

        ShareContent wechatMomentParams = new ShareContent();
        wechatMomentParams.SetImagePath(path);
        wechatMomentParams.SetTitle("AR游");//AR游
        wechatMomentParams.SetShareType(ContentType.Image);
        wechatMomentParams.SetShareContentCustomize(PlatformType.WeChatMoments, wechatParams);

        ShareContent SinaShareParams = new ShareContent();
        SinaShareParams.SetText("视觉美景+智能呈现  只留精彩，不留遗憾");
        SinaShareParams.SetImagePath(path);
        SinaShareParams.SetShareType(ContentType.Image);
        SinaShareParams.SetShareContentCustomize(PlatformType.SinaWeibo, SinaShareParams);

        //通过分享菜单分享
        ssdk.ShowPlatformList(plats, content, 100, 100);
        return true;
    }

    internal bool Share(string savedPath  )
    {
       
   
            if (GlobalParameter.isNeedRestore)
            {
                return ShareLink(savedPath);

            }
            else
            {
                return ShareImage(savedPath);
            }
        
    }



    /// <summary>
    /// 分享视频
    /// </summary>
    /// <param name="path"></param>
    public bool ShareVideo(string path)
    {
        if (path == null) return  false;
        Debug.Log("path===" + path);


        PlatformType[] plats = new PlatformType[1];
        //plats[0] = PlatformType.WeChat;
        //plats[1] = PlatformType.WeChatMoments;

        plats[0] = PlatformType.SinaWeibo;
        ShareContent content = new ShareContent();

        ssdk.DisableSSO(false);
        if (ssdk.IsClientValid(PlatformType.SinaWeibo)==false)
        {
            return false;
        }
        content.SetText("视觉美景+智能呈现  只留精彩，不留遗憾");//
        content.SetFilePath(path);
        content.SetTitle("AR游");//AR游
 
        content.SetShareType(ContentType.File);
        content.SetTitleUrl("http://download.vszapp.com");
        content.SetUrl("http://download.vszapp.com");

        //通过分享菜单分享
        ssdk.ShowPlatformList(plats, content, 100, 100);

        return true;
    }

    public void QQShare(string path)
    {

        ShareContent content = new ShareContent();
        content.SetImagePath(path);
        content.SetShareType(ContentType.Image);
        ssdk.ShareContent(PlatformType.QQ, content);
    }
    public void WEShare(string path)
    {

        ShareContent content = new ShareContent();
        content.SetText("this is a test string.");
        content.SetTitle("test title");
        content.SetImagePath(path);
        content.SetShareType(ContentType.Image);
        ssdk.ShareContent(PlatformType.WeChat, content);
    }
    public void WeMomentShare(string path)
    {

        ShareContent content = new ShareContent();
        content.SetText("this is a test string.");
        content.SetTitle("test title");
        content.SetImagePath(path);
        content.SetShareType(ContentType.Image);
        ssdk.ShareContent(PlatformType.WeChatMoments, content);
    }

    public void SinaWeiboShare(string path)
    {

        ShareContent content = new ShareContent();
        content.SetText("this is a test string.");
        content.SetTitle("test title");
        content.SetImagePath(path);
        content.SetShareType(ContentType.Image);
        ssdk.ShareContent(PlatformType.SinaWeibo, content);
    }



}
