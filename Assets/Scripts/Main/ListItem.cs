using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ListItem : MonoBehaviour {
    public RawImage jpg;
    /// <summary>
    /// ����ID
    /// </summary>
    public int id { get; set; }
    /// <summary>
    /// ��������
    /// </summary>
    public string name { get; set; }
    /// <summary>
    /// ��������ͼ��Ϣ
    /// </summary>
    public Thumbnail thumbnail { get; set; }
    /// <summary>
    /// ���㾭��
    /// </summary>
    public string locationX { get; set; }
    /// <summary>
    /// ����γ��
    /// </summary>
    public string locationY { get; set; }
    /// <summary>
    /// ���㺣��
    /// </summary>
    public string height { get; set; }
    /// <summary>
    /// �����ľ���
    /// </summary>
    public SceneryArea sceneryArea { get; set; }
    /// <summary>
    /// ��ҳ�����ַ
    /// </summary>
    public string address { get; set; }
    webrequest web;
    private void Start()
    {
        web = GameObject.Find("UniWebView").GetComponent<webrequest>();
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
                    DebugManager.Instance.LogError("����ʧ�ܣ�");
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
    /// ����ļ��Ƿ����
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