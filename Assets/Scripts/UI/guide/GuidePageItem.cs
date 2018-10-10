using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GuidePageItem : MonoBehaviour
{
    public RawImage png;
    private List<Texture> AllTex = new List<Texture>();

    public string Url;
    // Use this for initialization
    public void _init (string assetpath)
    {
        //GetImageList(assetpath);
        StartCoroutine(LoadImgFromCache(assetpath));
    }
    /// <summary>
    /// ��ȡͼƬ�б�
    /// </summary>
    void GetImageList(string assetpath)
    {
        //1.�ҵ���Դ������ļ���
        //string assetDirectory = Application.dataPath + "/DownloadFile/preview/" + Imgid.ToString();
        string assetDirectory = PublicAttribute.LocalFilePath + assetpath;
        DirectoryInfo directoryInfo = new DirectoryInfo(assetDirectory);

        if (directoryInfo == null)
        {
            Debug.LogError(directoryInfo + " ������!");
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
