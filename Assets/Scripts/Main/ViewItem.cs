using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ViewItem : MonoBehaviour
{
    public static ViewItem instance;
    private RawImage raw;
    private int _pid, _id;
    private List<Texture> AllTex = new List<Texture>();
    private Button btn;
    void Awake()
    {
        instance = this;
        raw = GetComponent<RawImage>();
        btn = GetComponent<Button>();
        btn.onClick.AddListener(EnterPlayScene);
    }

    public void ShowImage(int pid, int id)
    {
           _pid = pid;
        _id = id;
        GetImageList(id);
    }

    public void EnterPlayScene()
    {
        SceneManager.LoadScene("play");
    }

    /// <summary>
    /// 获取图片列表
    /// </summary>
    void GetImageList(int Imgid)
    {
        //1.找到资源保存的文件夹
        string assetDirectory = Application.dataPath + "/DownloadFile/preview/"+ Imgid.ToString();

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
                if (suffix == "jpg")
                {
                    string ImgName = file.Name.Substring(0,1);
                    for (int i = 1; i < 10; i++)
                    {
                        if (ImgName == i.ToString())
                        {
                            StartCoroutine(LoadImgFromCache(file.FullName,i));
                            StartCoroutine(ChangeImg());
                        }
                    }
                }
            }
        }
    }

    private IEnumerator LoadImgFromCache(string imgURl, int i)
    {
        if (CheckCacheUrlIsExit(imgURl))
        {
            Texture2D tex = new Texture2D(256, 256, TextureFormat.ARGB32, false);
            AllTex.Add(tex);
            if (i == 1)
                raw.texture = tex;
            HttpBase.Download("file:///" + imgURl, ((request, downloaded, length) => { }), ((request, response)
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
            raw.texture = AllTex[i];
        }
        yield return new WaitForSeconds(2);
        StartCoroutine(ChangeImg());
    }

    public void BackMain()
    {
        StopAllCoroutines();
        AllTex.Clear();
        raw.texture = null;
        ScrollList.instance.dot.DOPlayBackwards();
    }
}
