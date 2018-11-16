using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ListItem : MonoBehaviour {
    public RawImage jpg;
    public GameObject[] icons;
    /// <summary>
    /// 景点ID
    /// </summary>
    public int id { get; set; }
    /// <summary>
    /// 景点名称
    /// </summary>
    public string name { get; set; }
    /// <summary>
    /// 景点缩略图信息
    /// </summary>
    public Thumbnail thumbnail { get; set; }
    /// <summary>
    /// 景点经度
    /// </summary>
    public string locationX { get; set; }
    /// <summary>
    /// 景点纬度
    /// </summary>
    public string locationY { get; set; }
    /// <summary>
    /// 景点海拔
    /// </summary>
    public string height { get; set; }
    /// <summary>
    /// 所属的景区
    /// </summary>
    public SceneryArea sceneryArea { get; set; }
    /// <summary>
    /// 网页详情地址
    /// </summary>
    public string address { get; set; }
    
    /// <summary>
    /// 图标
    /// </summary>
    public string dynamicFlag { get; set; }
    
    webrequest web;
    private void Start()
    {
        web = GameObject.Find(GlobalInfo.websiterequest).GetComponent<webrequest>();
        if (dynamicFlag.Contains("1"))
        {
            icons[2].SetActive(true);
        }
        if (dynamicFlag.Contains("2"))
        {
            icons[3].SetActive(true);
        }
        if (dynamicFlag.Contains("3"))
        {
            icons[0].SetActive(true);
        }
        if (dynamicFlag.Contains("4"))
        {
            icons[1].SetActive(true);
        }
        
        //Debug.Log(thumbnail.localPath);

        //HttpManager.Instance.Download(thumbnail, (() =>
        //{
        //    _init(thumbnail.localPath);
        //}));
    }
    public void _init(string assetpath)
    {
        StartCoroutine(LoadImgFromCache(assetpath, jpg));
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
                request.Dispose();
                response.Dispose();
            }));
            yield return new WaitForEndOfFrame();
            
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
    public void OpenWeb()
    {
        web.LoadWebSetTitle(address,name);
    }
}