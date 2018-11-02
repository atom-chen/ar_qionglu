using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpotItem : MonoBehaviour
{
    /// <summary>
    /// 景点ID
    /// </summary>
    public int id;
    /// <summary>
    /// 景点名称
    /// </summary>
    public string name;
    /// <summary>
    /// 景点缩略图信息
    /// </summary>
    public Thumbnail thumbnail;
    /// <summary>
    /// 景点经度
    /// </summary>
    public string locationX;
    /// <summary>
    /// 景点纬度
    /// </summary>
    public string locationY;
    /// <summary>
    /// 景点海拔
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

    public string LocalAddress;
    public RawImage img;
    public Text nameTxt;
    webrequest web;
    public void Start()
    {
        web = GameObject.Find(GlobalInfo.websiterequest).GetComponent<webrequest>();
        nameTxt.text = name;
        _init(thumbnail.localPath);
        LocalAddress = thumbnail.localPath;
    }
    public void _init(string assetpath)
    {
        StartCoroutine(LoadImgFromCache(assetpath, img));
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

    public void OpenWeb()
    {
        web.LoadWebSetTitle(address,name);
    }
}
