using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class BananrItem : MonoBehaviour {

    public RawImage jpg;
    public RawImage png;
    public VideoPlayer mp4;
    public string address;
    webrequest web;
    // Use this for initialization
    public void _init(string assetpath)
    {
        web = GameObject.Find("UniWebView").GetComponent<webrequest>();
        StartCoroutine(LoadImgFromCache(assetpath, jpg));
    }

    private IEnumerator LoadImgFromCache(string imgURl, RawImage img)
    {
        //Debug.Log(imgURl);
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
        Debug.Log(address);
        web.LoadWeb(address);
    }
}
