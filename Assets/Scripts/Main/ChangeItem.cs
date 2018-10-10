using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Linq;

public class ChangeItem : MonoBehaviour {

    public RawImage png;

    public Text title,info;
    public Vector2 GPS;
    /// <summary>
    /// 全景点详情
    /// </summary>
    public string content;
    /// <summary>
    /// 全景点ID
    /// </summary>
    public string id;
    /// <summary>
    /// 全景点名称
    /// </summary>
    public string name;
    /// <summary>
    /// 全景点缩略图信息
    /// </summary>
    public Thumbnail thumbnail;
    /// <summary>
    /// 全景点经度
    /// </summary>
    public string locationX;
    /// <summary>
    /// 全景点纬度
    /// </summary>
    public string locationY;
    /// <summary>
    /// 全景点海拔
    /// </summary>
    public string height;
    /// <summary>
    /// 所属的景区
    /// </summary>
    public SceneryArea sceneryArea;
    /// <summary>
    /// 网页详情地址
    /// </summary>
    public string address;
    /// <summary>
    /// 所有的资源
    /// </summary>
    public List<VersionFilesItem> VersionFilesItems;

    public string LocalAddress;
    webrequest web;
    public void Start()
    {
        web = GameObject.Find("UniWebView").GetComponent<webrequest>();
        info.text = content;
        title.text = name;
        LocalAddress = thumbnail.localPath;
        _init(thumbnail.localPath);
    }
    // Use this for initialization
    public void _init(string assetpath)
    {
        info.text = content;
        title.text = name;
        StartCoroutine(LoadImgFromCache(assetpath, png));
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

    public void BtnOnClick()
    {
        Debug.Log(locationX);
        Debug.Log(locationY);
        //if (Vector2.Distance(GPS, new Vector2(float.Parse(locationX), float.Parse(locationY))) < 10)
        //{
        //    foreach (var item in VersionFilesItems)
        //    {
        //        Debug.Log(item.localPath);
        //        StartCoroutine(LoadAssets(item.localPath));
        //    }
        //}
        //else
        //{
        //    foreach (var item in VersionFilesItems)
        //    {
        //        Debug.Log(item.localPath);
        //        StartCoroutine(LoadAssets(item.localPath));
        //    }
        //}
        foreach (var item in VersionFilesItems.Where(item=> item.extName=="vsz"))
        {
            Debug.Log(item.localPath);
            StartCoroutine(LoadAssets(item.localPath));
        }
    }

    public void OpenWeb()
    {
        web.LoadWeb(address);
    }
    private IEnumerator LoadAssets(string path)
    {
        Debug.Log(File.Exists(path));
        WWW bundle = WWW.LoadFromCacheOrDownload("file:///" + path, 0);
        yield return bundle;
        Debug.Log(bundle.size);
        int index1 = path.LastIndexOf("/");
        int index2 = path.LastIndexOf(".");
        string suffix = path.Substring(index1+1, index2-index1-1);
        var data = bundle.assetBundle;
        SceneManager.LoadScene(suffix);
    }
}
