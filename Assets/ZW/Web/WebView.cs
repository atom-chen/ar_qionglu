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
        var assetUrl = UniWebViewHelper.PersistentDataURLForPath("DownloadFile/Web/CustomOverlay.html");

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
        webView.CleanCache();
        Destroy(webView.gameObject);
        webView = null;



    }
        

    /// <summary>
    /// 给地图生成点
    /// </summary>
    public void SpawnPointToMap()
    {

        LinkList pointLinkList = TrackDataManager.Instance.pointLinkList;
        ListNode node = new ListNode();
        int length = pointLinkList.Length();
        for (int i = 1; i <= length; i++)
        {
            node = pointLinkList.GetNode(i);
            string longitude = node.longitude;
            string latitude = node.latitude;
            string path = node.path;
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
        if (Input.GetKeyDown(KeyCode.A))
        {
           TrackDataManager.Instance. AddPointToPointClass(2, "333", "444", "asda.pngg", "2018|06|19");
            TrackDataManager.Instance.AddPointToPointClass(4, "333", "444", "zw.png", "2018|10|19");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            TrackDataManager.Instance.SaveStringToFile();
        }
    }
 





}
