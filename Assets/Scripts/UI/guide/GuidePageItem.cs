using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GuidePageItem : MonoBehaviour
{
    public RawImage png;
    public Image box;
    private List<Texture> AllTex = new List<Texture>();

    public string Url;

    private void Start()
    {
        #if UNITY_ANDROID
        GetComponent<RectTransform>().sizeDelta=new Vector2(Screen.width,Screen.height);
        box.GetComponent<RectTransform>().sizeDelta=new Vector2(Screen.width,Screen.height);
        box.GetComponent<RectTransform>().anchoredPosition=new Vector2(0,(1920-Screen.height)/2f);
        #elif UNITY_IOS || UNITY_IPHONE

        #endif
    }

    // Use this for initialization
    public void _init (string assetpath)
    {
        //GetImageList(assetpath);
        StartCoroutine(LoadImgFromCache(assetpath));
    }
    /// <summary>
    /// 获取图片列表
    /// </summary>
    void GetImageList(string assetpath)
    {
        //1.找到资源保存的文件夹
        //string assetDirectory = Application.dataPath + "/DownloadFile/preview/" + Imgid.ToString();
        string assetDirectory = PublicAttribute.LocalFilePath + assetpath;
        DirectoryInfo directoryInfo = new DirectoryInfo(assetDirectory);

        if (directoryInfo == null)
        {
            Debug.LogError(directoryInfo + " 不存在!");
            return;
        }
        else
        {
            foreach (FileInfo file in directoryInfo.GetFiles("*"))
            {
                int index = file.FullName.LastIndexOf(".");
                string suffix = file.FullName.Substring(index + 1);
                if (suffix != "meta")
                {
                    StartCoroutine(LoadImgFromCache(assetDirectory + "/" + file.Name));
                }
            }
            StartCoroutine(ChangeImg());
        }
    }

    private bool first;
    private IEnumerator LoadImgFromCache(string imgURl)
    {
        if (CheckCacheUrlIsExit(imgURl))
        {
            Texture2D tex = new Texture2D(256, 256, TextureFormat.ARGB32, false);
            AllTex.Add(tex);
            if (!first)
            {
                first = true;
                png.texture = tex;
            }
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

    IEnumerator ChangeImg()
    {
        for (int i = 0; i < AllTex.Count; i++)
        {
            yield return new WaitForSeconds(2);
            png.texture = AllTex[i];
        }
        yield return new WaitForSeconds(2);
        StartCoroutine(ChangeImg());
    }
}
