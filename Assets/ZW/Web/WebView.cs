using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using System.Text;
using UnityEngine.UI;



public class WebView : SingletonMono<WebView>
{
    public RectTransform WebUI;
    [HideInInspector]
    public UniWebView webView;

    string lastId = string.Empty;


    /// <summary>
    /// 加载并显示WEB UI
    /// </summary>
    public void CreateWebView()
    {
        webView = UniWebViewHelper.CreateWebView();
        //1.找到资源保存的文件夹
#if UNITY_ANDROID
      //  var assetUrl = "/sdcard/DCIM/AR游/CustomOverlay.html";
        var assetUrl = UniWebViewHelper.PersistentDataURLForPath("DownloadFile/Web/CustomOverlay.html");
#elif  UNITY_IOS||UNITY_IPHONE
             var assetUrl = UniWebViewHelper.PersistentDataURLForPath("DownloadFile/Web/CustomOverlay.html");
#endif

        Debug.Log("web view url =====" + assetUrl);
        webView.ReferenceRectTransform = WebUI;
        webView.urlOnStart = assetUrl;
        webView.Load(assetUrl);
        webView.Show();

        webView.OnMessageReceived += WebView_OnMessageReceived;
        webView.OnPageFinished += WebView_OnPageFinished;
        webView.OnPageStarted += WebView_OnPageStarted;
        webView.OnPageErrorReceived += WebView_OnPageErrorReceived;

    }
    /// <summary>
    /// 页面加载开始
    /// </summary>
    /// <param name="webView"></param>
    /// <param name="url"></param>
    private void WebView_OnPageStarted(UniWebView webView, string url)
    {
        try
        {
            TrackUIManager.Instance.LoadStart();
        }
        catch (System.Exception ex)
        {

        }
    }

    /// <summary>
    /// 页面加载出错
    /// </summary>
    /// <param name="webView"></param>
    /// <param name="errorCode"></param>
    /// <param name="errorMessage"></param>
    private void WebView_OnPageErrorReceived(UniWebView webView, int errorCode, string errorMessage)
    {
        try
        {
            TrackUIManager.Instance.LoadError();
        }
        catch (System.Exception ex)
        {

        }
    }



    /// <summary>
    /// 页面加载完毕回调事件
    /// </summary>
    private void WebView_OnPageFinished(UniWebView webView, int statusCode, string url)
    {
    
        Debug.Log("加载完毕" + statusCode + "***");
        try
        {
            TrackUIManager.Instance.LoadComplete();
           
            String location = UnityHelper.GetBDGPSLocation();
            JsonData zb = JsonMapper.ToObject(location);
            string x = zb["longitude"].ToString();
            string y = zb["latitude"].ToString();
   
            string jsString = @"PanTo(" + x + "," + y + ");";
            Debug.Log(jsString);

            webView.EvaluateJavaScript(jsString, CompletionHandler);
        }
        catch (System.Exception ex)
        {
        	    
        }


     SpawnPointToMap();
    }

    /// <summary>
    /// 页面按钮点击事件回调
    /// </summary>
    private void WebView_OnMessageReceived(UniWebView webView, UniWebViewMessage message)
    {
        string index = message.Args["index"];

        Debug.Log("覆盖物" + index + "     ***");

        ClearWebView();
         GalleryImageManager.Instance.SpawnImage(index);
        TrackUIManager.Instance.SetTitleText("相册", 1);

    }
    /// <summary>
    /// 清除地图
    /// </summary>
    public void ClearWebView()
    {
        if (webView!=null)
        {
            webView.CleanCache();
            Destroy(webView.gameObject);
            webView = null;
        }




    }
        

    /// <summary>
    /// 给地图生成点
    /// </summary>
    public void SpawnPointToMap()
    {

        LinkList pointLinkList = LoadTrackData.Instance.pointLinkList;
        ListNode node = new ListNode();
        int length = pointLinkList.Length();
        for (int i = 1; i <= length; i++)
        {
            node = pointLinkList.GetNode(i);
            string longitude = node.longitude;
            string latitude = node.latitude;
            string path = node.path;
#if   UNITY_ANDROID
            path = "/sdcard/DCIM/Camera/" + path;
#elif UNITY_IOS || UNITY_IPHONE
                path = "/"+Application.persistentDataPath+"/" + path;
#endif
            string count = node.count;
            string id = node.id;
            string jsString = @"PointOverlap(" + longitude + "," + latitude + "," + "\"" + path + "\"" + "," + count + "," + id + ");";
            Debug.Log(jsString);

            webView.EvaluateJavaScript(jsString, CompletionHandler);
      
        }
    }
    /// <summary>
    /// js运行回调
    /// </summary>
    /// <param name="obj"></param>
    private void CompletionHandler(UniWebViewNativeResultPayload obj)
    {
        if (obj.resultCode != "0")
        {
            Debug.LogError("webView 运行js脚本出错，请检查参数");
          
        }
        else
        {
            //  messageText.text += obj.resultCode + "\n";
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TrackDataManager.Instance.AddPointToPointClass("333", "444", "asda.pngg", "20180619");

        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {

            TrackDataManager.Instance.AddPointToPointClass("333", "444", "zw.png", "20181019");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {

            TrackDataManager.Instance.AddPointToPointClass("333", "444", "sssss.png", "20181019");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            TrackDataManager.Instance.SaveStringToFile();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            ClearWebView();
            GalleryImageManager.Instance.SpawnImage("1");
            TrackUIManager.Instance.SetTitleText("相册", 1);
        }
    }






}
